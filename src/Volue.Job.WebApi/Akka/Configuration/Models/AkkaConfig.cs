namespace Volue.Job.WebApi.Models
{
    public class AkkaConfig
    {
        public string SystemName { get; set; }
        public string CalculationServiceSystemName { get; set; }
        public string CalculationServicePort { get; set; }
        public int ResponseTimeout { get; set; }
        public string CalculationServiceHostname { get; set; }
    }
}
