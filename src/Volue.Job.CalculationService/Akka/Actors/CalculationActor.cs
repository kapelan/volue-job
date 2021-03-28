using Akka.Actor;
using System;
using System.Linq;
using Volue.Job.Messages;

namespace Volue.Job.CalculationService.Akka.Actors
{
    public class CalculationActor : ReceiveActor
    {
        public CalculationActor()
        {
            Receive<CalculateSum>(msg =>
            {
                try
                {
                    var total = msg.Values.Sum(item => item);
                    Sender.Tell(new Complete.Success(total), Self);
                }
                catch (Exception ex)
                {
                    Sender.Tell(new Complete.Failure(ex.Message), Self);
                    throw;
                }
            });
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
    }
}
