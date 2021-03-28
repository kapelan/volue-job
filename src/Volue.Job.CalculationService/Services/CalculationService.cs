using Akka.Actor;
using Akka.DI.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Akka.Routing;
using Volue.Job.CalculationService.Akka.Actors;
using Volue.Job.CalculationService.Akka.Configuration;
using Volue.Job.CalculationService.Akka.Configuration.Models;
using Volue.Job.CalculationService.Akka.DependencyInjection;
using Volue.Job.Common.Exceptions;

namespace Volue.Job.CalculationService.Services
{
    public class CalculationService : IHostedService
    {
        private ActorSystem _system;
        private IServiceProvider _serviceProvider;

        public CalculationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var configurationProvider = _serviceProvider.GetService<IAkkaConfigurationProvider>();

            var options = _serviceProvider.GetService<AkkaConfig>();
            if (string.IsNullOrEmpty(options?.SystemName)) throw new AkkaConfigConfigurationNotFoundException();

            _system = ActorSystem.Create(options?.SystemName, configurationProvider.ProvideHocon());
            _system.UseServiceProvider(_serviceProvider);
            _system.ActorOf(_system.DI().Props<MonitorActor>(), "monitor");

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _system.Terminate();
        }
    }
}
