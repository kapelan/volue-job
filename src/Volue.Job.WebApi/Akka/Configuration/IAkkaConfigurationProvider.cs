namespace Volue.Job.WebApi.Configuration
{
    public interface IAkkaConfigurationProvider
    {
        string ProvideHocon();
    }
}
