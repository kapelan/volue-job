using System;

namespace Volue.Job.Persistance.Entities
{
    public class DataPoint
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public long Epoch { get; set; }
        public double Value { get; set; }
    }
}
