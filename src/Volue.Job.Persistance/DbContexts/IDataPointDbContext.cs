using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Volue.Job.Persistance.DbContexts
{
    public interface IDataPointDbContext : IDisposable
    {
        DbSet<Entities.DataPoint> DataPoints { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
        int SaveChanges();
    }
}
