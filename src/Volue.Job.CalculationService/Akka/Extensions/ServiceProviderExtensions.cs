using Microsoft.Extensions.DependencyInjection;
using System;
using Volue.Job.Common.Exceptions;

namespace Volue.Job.CalculationService.Akka.Extensions
{
    public static class ServiceProviderExtensions
    {
        public static T Get<T>(this IServiceProvider serviceProvider) where T : class
            => serviceProvider.GetService<T>() ?? throw new ServiceNotFoundException<T>();
    }
}
