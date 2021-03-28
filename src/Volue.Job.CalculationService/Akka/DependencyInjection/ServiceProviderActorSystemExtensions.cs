using System;
using System.Collections.Generic;
using System.Text;
using Akka.Actor;
using Akka.DI.Core;

namespace Volue.Job.CalculationService.Akka.DependencyInjection
{
    public static class ServiceProviderActorSystemExtensions
    {
        public static ActorSystem UseServiceProvider(this ActorSystem system, IServiceProvider serviceProvider)
            => system.UseServiceProvider(serviceProvider, out _);

        public static ActorSystem UseServiceProvider(this ActorSystem system, IServiceProvider serviceProvider,
            out IDependencyResolver dependencyResolver)
        {
            if (system == null)
            {
                throw new ArgumentNullException(nameof(system));
            }

            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            dependencyResolver = new ServiceProviderDependencyResolver(serviceProvider, system);
            return system;
        }
    }
}
