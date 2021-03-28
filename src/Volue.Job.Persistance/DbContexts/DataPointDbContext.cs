using Microsoft.EntityFrameworkCore;
using Volue.Job.Persistance.Extensions;

namespace Volue.Job.Persistance.DbContexts
{
    public class DataPointDbContext : DbContext, IDataPointDbContext
    {
        public DataPointDbContext(DbContextOptions<DataPointDbContext> options) : base(options)
        {
        }

        public DbSet<Entities.DataPoint> DataPoints { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataPointDbContext).Assembly);
            modelBuilder.RemovePluralizingTableNameConvention();
        }
    }
}
