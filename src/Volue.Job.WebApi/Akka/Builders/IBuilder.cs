namespace Volue.Job.WebApi.Builders
{
    public interface IBuilder<out T>
    {
        T Build();
    }
}
