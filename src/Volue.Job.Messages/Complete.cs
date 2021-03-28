namespace Volue.Job.Messages
{
    public abstract class Complete
    {
        public class Success : Complete
        {
            public readonly object Result;

            public Success(object result) => Result = result;
        }

        public class Failure : Complete
        {
            public readonly string Reason;

            public Failure(string reason = null) => Reason = reason;
        }

        public class NothingChanged : Complete
        {
            public readonly object Reason;

            public NothingChanged(object reason = null) => Reason = reason;
        }
    }
}
