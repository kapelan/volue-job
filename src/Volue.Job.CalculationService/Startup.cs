using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Volue.Job.CalculationService.Akka.Actors;
using Volue.Job.CalculationService.Akka.Configuration;
using Volue.Job.CalculationService.Akka.Configuration.Models;
using Volue.Job.Persistance.DbContexts;

namespace Volue.Job.CalculationService
{
    class Startup
    {
        public static IConfiguration StaticConfig { get; private set; }

        public static void ConfigureServices(HostBuilderContext hostBuilderContext, IServiceCollection services)
        {
            var configuration = hostBuilderContext.Configuration;
            StaticConfig = configuration;

            services.AddDataProtection();

            services.AddLogging(configure => configure.AddSerilog(dispose: true));

            var connectionString = configuration.GetConnectionString("DataPointDbContext");

            services.AddDbContext<IDataPointDbContext, DataPointDbContext>(options =>
                ((DbContextOptionsBuilder<DataPointDbContext>)options)
                .UseSqlServer(connectionString));

            services.AddSingleton<AkkaConfig>(configuration.GetSection("AkkaConfig").Get<AkkaConfig>());

            services.AddScoped<MonitorActor>();

            services.AddScoped<CalculationActor>();

            if (hostBuilderContext.HostingEnvironment.IsEnvironment("DockerDev"))
            {
                services.AddTransient<IAkkaConfigurationProvider>(
                    x => new AkkaConfigurationProvider("akka.docker.xml"));
            }
            else
            {
                services.AddTransient<IAkkaConfigurationProvider>(
                    x => new AkkaConfigurationProvider("akka.xml"));
            }

            services.AddHostedService<Services.CalculationService>();
        }
    }
}
