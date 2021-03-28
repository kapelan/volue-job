using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Volue.Job.CalculationService
{
    class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddCommandLine(args)
                .Build();
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config, "Serilog")
                .CreateLogger();

            var host = Host.CreateDefaultBuilder();
            if (string.IsNullOrEmpty(config["DisableWindowsService"]))
                host.UseWindowsService();

            return host.ConfigureServices(Startup.ConfigureServices);
        }
    }
}
