namespace Volue.Job.CalculationService.Akka.Configuration
{
    public interface IAkkaConfigurationProvider
    {
        string ProvideHocon();
    }
}
