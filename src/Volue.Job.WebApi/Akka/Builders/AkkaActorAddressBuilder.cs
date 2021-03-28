using System.IO;

namespace Volue.Job.WebApi.Builders
{
    public class AkkaActorAddressBuilder : IBuilder<string>
    {
        private string _system;
        private int? _port;
        private string _actor;
        private string _hostname;

        public AkkaActorAddressBuilder WithSystem(string system)
        {
            _system = system;
            return this;
        }
        public AkkaActorAddressBuilder WithActor(string actor)
        {
            _actor = actor;
            return this;
        }
        public AkkaActorAddressBuilder WithPort(int port)
        {
            _port = port;
            return this;
        }

        public AkkaActorAddressBuilder WithHostname(string hostname)
        {
            _hostname = hostname;
            return this;
        }

        public string Build()
        {
            if (string.IsNullOrWhiteSpace(_system)) throw new InvalidDataException("System value is invalid.");
            if (string.IsNullOrWhiteSpace(_actor)) throw new InvalidDataException("Actor value is invalid.");
            if (_port == null) throw new InvalidDataException("Port value cannot be null.");
            if (string.IsNullOrWhiteSpace(_hostname)) throw new InvalidDataException("Hostname value is invalid.");

            return $"akka.tcp://{_system}@{_hostname}:{_port}/user/{_actor}";
        }
    }
}
