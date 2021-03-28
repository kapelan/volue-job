using System;
using System.Collections.Generic;
using System.Text;

namespace Volue.Job.Common.Exceptions
{
    public class AkkaConfigConfigurationNotFoundException : Exception
    {
        private static string Message = "AkkaConfig configuration section was not found in appsettings";
        public AkkaConfigConfigurationNotFoundException() : base(Message)
        {
        }
    }
}
