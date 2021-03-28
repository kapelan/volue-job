using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Volue.Job.CalculationService.Akka.Configuration
{
    public class AkkaConfigurationProvider : IAkkaConfigurationProvider
    {
        private readonly string _configFile;

        public AkkaConfigurationProvider(string configFile)
        {
            _configFile = configFile ?? throw new ArgumentNullException(nameof(configFile));
        }
        public string ProvideHocon()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddXmlFile(_configFile);
            var akkaConfig = builder.Build();
            return akkaConfig.GetSection(String.Empty).Value;
        }
    }
}
