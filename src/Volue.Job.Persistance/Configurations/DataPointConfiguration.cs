using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volue.Job.Persistance.Entities;

namespace Volue.Job.Persistance.Configurations
{
    public class DataPointConfiguration : IEntityTypeConfiguration<DataPoint>
    {
        public void Configure(EntityTypeBuilder<DataPoint> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }
}
