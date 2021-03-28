namespace Volue.Job.Messages
{
    public class CalculateDataPoint
    {
         public string DataPointName { get; set; }
         public long? From { get; set; }
         public long? To { get; set; }
    }
}
