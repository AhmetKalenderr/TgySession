using Microsoft.EntityFrameworkCore;
using SessionService.Entities;

namespace SessionService.Models
{
    public class TgyDatabaseContext : DbContext
    {
        public TgyDatabaseContext(DbContextOptions<TgyDatabaseContext> options) : base(options)
        {

        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Segment> Segments { get; set; }


        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<Customer>()
                .HasOne(c => c.segment)
                .WithMany()
                .HasForeignKey(s=>s.SegmentId);
        }     
    }   
}
