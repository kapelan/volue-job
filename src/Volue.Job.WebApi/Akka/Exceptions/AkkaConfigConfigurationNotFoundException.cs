using System;

namespace Volue.Job.WebApi.Exceptions
{
    public class AkkaConfigConfigurationNotFoundException : Exception
    {
        private static string Message = "AkkaConfig configuration section was not found in appsettings";
        public AkkaConfigConfigurationNotFoundException() : base(Message)
        {
        }
    }
}
