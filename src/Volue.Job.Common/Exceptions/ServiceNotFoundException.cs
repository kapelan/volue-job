using System;
using System.Collections.Generic;
using System.Text;

namespace Volue.Job.Common.Exceptions
{
    public class ServiceNotFoundException<T> : Exception where T : class
    {
        public ServiceNotFoundException() : base($"Service: {typeof(T).FullName} cannot be provided")
        {
        }
    }
}
