using System;
using Akka.Actor;
using Akka.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Volue.Job.WebApi.Akka.Actors;
using Volue.Job.WebApi.Builders;
using Volue.Job.WebApi.Configuration;
using Volue.Job.WebApi.Exceptions;
using Volue.Job.WebApi.Models;

namespace Volue.Job.WebApi.Extensions
{
    public static class AkkaSystemExtensions
    {
        public static void ConfigureActors(this IApplicationBuilder app)
        {
            var serviceProvider = app.ApplicationServices;

            var hoconConfig = LoadConfiguration(serviceProvider, out var options);

            CreateAkkaSystem(options, hoconConfig);
        }

        private static void CreateAkkaSystem(AkkaConfig options, string hoconConfig)
        {
            var readerAddressBuilder = new AkkaActorAddressBuilder()
                .WithActor("monitor")
                .WithPort(int.Parse(options.CalculationServicePort))
                .WithSystem(options.CalculationServiceSystemName)
                .WithHostname(options.CalculationServiceHostname);


            AkkaSystemKeeper.System = ActorSystem.Create(options.SystemName, ConfigurationFactory.ParseString(hoconConfig));
            AkkaSystemKeeper.ReaderActor = AkkaSystemKeeper.System.ActorOf(Props.Create(() =>
                new ReaderActor(readerAddressBuilder, options.ResponseTimeout)), "reader");
        }

        private static string LoadConfiguration(IServiceProvider serviceProvider, out AkkaConfig options)
        {
            var configurationProvider = serviceProvider.GetService<IAkkaConfigurationProvider>();
            var hoconConfig = configurationProvider.ProvideHocon();

            options = serviceProvider.GetService<AkkaConfig>();
            if (string.IsNullOrEmpty(options?.SystemName)) throw new AkkaConfigConfigurationNotFoundException();

            return hoconConfig;
        }

        public static void AddAkkaConfiguration(this IServiceCollection serviceCollection, IConfiguration configuration, IHostEnvironment env)
        {
            serviceCollection.AddSingleton<AkkaConfig>(configuration.GetSection("WebApiAkkaConfig").Get<AkkaConfig>());
           
            if (env.IsEnvironment("DockerDev"))
            {
                serviceCollection.AddTransient<IAkkaConfigurationProvider>(
                    x => new AkkaConfigurationProvider("akka.webapi.docker.xml"));
            }
            else
            {
                serviceCollection.AddTransient<IAkkaConfigurationProvider>(
                    x => new AkkaConfigurationProvider("akka.webapi.xml"));
            }
        }
    }
}
