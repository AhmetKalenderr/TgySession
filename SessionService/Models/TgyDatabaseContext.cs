using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using SessionService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
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
            ChangeTracker.DetectChanges();

            IEnumerable<EntityEntry> entries = ChangeTracker.Entries().ToList();

            foreach (var entry in entries)
            {
                Console.WriteLine(entry.Entity.GetType().ToString());

                if (entry.Entity is Customer)
                {
                    Customer customer = (Customer)entry.Entity;
                    customer.segment = Segments.Find(customer.SegmentId);
                    Logs.Add(new Log() { ActionTime = DateTime.Now.ToLocalTime(), ActionType = entry.State == EntityState.Modified ? "Updated" : entry.State.ToString(), Data = JsonConvert.SerializeObject(customer), TableName = "Customer" });
                }
                else if (entry.Entity is Segment)
                {
                    Segment segment = (Segment)entry.Entity;
                    Logs.Add(new Log() { ActionTime = DateTime.Now.ToLocalTime(), ActionType = entry.State == EntityState.Modified ? "Updated" : entry.State.ToString(), Data = JsonConvert.SerializeObject(segment), TableName = "Segment" });
                }


            }

            return base.SaveChangesAsync();
        }
    }   
}
