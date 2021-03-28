using Akka.Actor;
using Akka.DI.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace Volue.Job.CalculationService.Akka.DependencyInjection
{
    public class ServiceProviderDependencyResolver
        : IDependencyResolver, INoSerializationVerificationNeeded
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly ActorSystem _system;

        private readonly ConcurrentDictionary<string, Type> _typeCache = new ConcurrentDictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

        private readonly ConditionalWeakTable<ActorBase, IServiceScope> _references = new ConditionalWeakTable<ActorBase, IServiceScope>();

        public ServiceProviderDependencyResolver(IServiceProvider serviceProvider, ActorSystem system)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            if (system == null)
            {
                throw new ArgumentNullException(nameof(system));
            }

            _serviceProvider = serviceProvider;
            _system = system;
            system.AddDependencyResolver(this);
        }

        public Type GetType(string actorName)
        {
            _typeCache.TryAdd(
                actorName,
                actorName.GetTypeValue());

            return _typeCache[actorName];
        }

        public Func<ActorBase> CreateActorFactory(Type actorType)
            => () =>
            {
                var scope = _serviceProvider.CreateScope();
                var actor = (ActorBase)scope.ServiceProvider.GetRequiredService(actorType);
                _references.Add(actor, scope);
                return actor;
            };

        public Props Create<TActor>()
            where TActor : ActorBase
            => Create(typeof(TActor));

        public Props Create(Type actorType)
            => _system.GetExtension<DIExt>().Props(actorType);

        public void Release(ActorBase actor)
        {
            if (_references.TryGetValue(actor, out var scope))
            {
                scope.Dispose();
                _references.Remove(actor);
            }
        }
    }
}
