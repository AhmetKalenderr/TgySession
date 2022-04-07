using Microsoft.EntityFrameworkCore;
using SessionService.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace SessionService.Models
{
    public class TgyDatabaseContext : DbContext
    {
        public TgyDatabaseContext(DbContextOptions<TgyDatabaseContext> options) : base(options)
        {

        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Segment> Segments { get; set; }

        public DbSet<Log> Logs { get; set; }


        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<Customer>()
                .HasOne(c => c.segment)
                .WithMany()
                .HasForeignKey(s=>s.SegmentId);
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }
    }   
}
