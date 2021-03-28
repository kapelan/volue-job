using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Volue.Job.Persistance.DbContexts;

namespace Volue.Job.DbMigrator
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration, "Serilog")
                .CreateLogger();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection, configuration);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var dbContext = (DataPointDbContext)serviceProvider.CreateScope().ServiceProvider.GetService<IDataPointDbContext>();

            dbContext.Database.Migrate();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DataPointMigrationDbContext");

            services.AddDbContext<IDataPointDbContext ,DataPointDbContext>(options =>
                ((DbContextOptionsBuilder<DataPointDbContext>)options)
                .UseSqlServer(connectionString));

            services.AddLogging(configure => configure.AddSerilog(dispose: true));
        }
    }
}
