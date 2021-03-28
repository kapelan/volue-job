using Akka.Actor;
using Akka.Routing;
using Polly;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volue.Job.CalculationService.Akka.Extensions;
using Volue.Job.Common.Exceptions;
using Volue.Job.Messages;
using Volue.Job.Persistance.DbContexts;

namespace Volue.Job.CalculationService.Akka.Actors
{
    public class MonitorActor : ReceiveActor
    {
        private readonly IDataPointDbContext _dbContext;
        private IActorRef _calculator;
        public MonitorActor(IDataPointDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

            AddCalculationActors();
            
            //TODO Refactor this massage handler, logic is too complex and hard to understand
            ReceiveAsync<CalculateDataPoint>(async msg =>
            {
                try
                {
                    Validate(msg);

                    var results = new ConcurrentBag<double>();
                    var bulkheadPolicy = Policy.BulkheadAsync(Environment.ProcessorCount);
                    var parallelTasks = new List<Task>();
                    var data = GetData(msg);

                    if (data == null || !data.Any())
                        Sender.Tell(new Complete.Success(new CalculationResult
                        {
                            Avg = 0,
                            Sum = 0
                        }), Self);

                    //TODO: add polly's retry policy 
                    foreach (var set in data.Batch(10))
                    {
                        var task = bulkheadPolicy.ExecuteAsync(async () =>
                        {
                            var response = await _calculator.Ask<Complete>(new CalculateSum
                            {
                                Values = set
                            });

                            if (response is Complete.Success success)
                                results.Add((double)success.Result);
                        });

                        parallelTasks.Add(task);
                    }

                    var whenAllTasks = Task.WhenAll(parallelTasks);

                    await whenAllTasks;

                    var sum = results.Sum();
                    var avg = sum / data.Count();

                    Sender.Tell(new Complete.Success(new CalculationResult
                    {
                        Sum = sum,
                        Avg = avg
                    }), Self);
                }
                catch (Exception ex)
                {
                    Sender.Tell(new Complete.Failure(ex.Message), Self);
                    throw;
                }
            });
        }

        private IEnumerable<double> GetData(CalculateDataPoint message)
        {
            if (message.From == null && message.To == null)
            {
                return _dbContext.DataPoints.Where(item => item.Name.Equals(message.DataPointName))
                    .Select(item => item.Value).ToList();
            }
            if (message.From == null && message.To != null)
            {
                return _dbContext.DataPoints.Where(item
                         => item.Name.Equals(message.DataPointName) && item.Epoch <= message.To)
                     .Select(item => item.Value).ToList();
            }
            if (message.From != null && message.To == null)
            {
                return _dbContext.DataPoints.Where(item
                        => item.Name.Equals(message.DataPointName) && item.Epoch >= message.From)
                    .Select(item => item.Value).ToList();
            }

            return _dbContext.DataPoints.Where(item
                    => item.Name.Equals(message.DataPointName) && item.Epoch >= message.From 
                                                               && item.Epoch <= message.To)
                .Select(item => item.Value).ToList();
        }

        private void Validate(CalculateDataPoint msg)
        {
            //TODO use fluentValidation instead
            if (string.IsNullOrWhiteSpace(msg.DataPointName))
                throw new ValidationException("DataPointName cannot be null or empty");
            if (msg.From > msg.To)
                throw new ValidationException("To must be greater than From");
            if (msg.From < 0 || msg.To < 0)
                throw new ValidationException("To and From must be greater than 0");
        }

        private void AddCalculationActors()
        {
            _calculator = Context.ActorOf(Props.Create<CalculationActor>().WithRouter(
                new RoundRobinPool(Environment.ProcessorCount)), "calculation");
        }

        //TODO supervising strategy need to cover all exception 
        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy(
                maxNrOfRetries: 10,
                withinTimeRange: TimeSpan.FromMinutes(1),
                localOnlyDecider: ex =>
                {
                    switch (ex)
                    {
                        default:
                            return Directive.Resume;
                    }
                });
        }

        protected override void PreStart()
        {
        }

        protected override void PreRestart(Exception reason, object message)
        {
            foreach (IActorRef each in Context.GetChildren())
            {
                Context.Unwatch(each);
                Context.Stop(each);
            }
            PostStop();
        }

        protected override void PostRestart(Exception reason)
        {
            PreStart();
        }

        protected override void PostStop()
        {
        }
    }
}
