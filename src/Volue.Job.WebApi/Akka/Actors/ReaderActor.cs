using System;
using Akka.Actor;
using Volue.Job.WebApi.Builders;

namespace Volue.Job.WebApi.Akka.Actors
{
    public class ReaderActor : ReceiveActor
    {
        private readonly IBuilder<string> _builder;
        public ReaderActor(IBuilder<string> builder, int timeout)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
            ReceiveAnyAsync(async message =>
            {
                var address = _builder.Build();
                var response = await Context.ActorSelection(address)
                    .Ask(message, TimeSpan.FromSeconds(timeout));
                Sender.Tell(response);
            });
        }
    }
}
