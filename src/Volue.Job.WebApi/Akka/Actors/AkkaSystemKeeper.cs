using Akka.Actor;

namespace Volue.Job.WebApi.Akka.Actors
{
    public class AkkaSystemKeeper
    {
        public static ActorSystem System { get; set; }
        public static IActorRef ReaderActor { get; set; }
    }
}
