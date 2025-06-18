using Microsoft.EntityFrameworkCore;
using TaskManagement.API.Models;

namespace TaskManagement.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Models.Task> Tasks => Set<Models.Task>();
        public DbSet<User> Users => Set<User>();
        public DbSet<ProcurementData> ProcurementData => Set<ProcurementData>();
        public DbSet<DevelopmentData> DevelopmentData => Set<DevelopmentData>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProcurementData>()
                .HasKey(p => p.TaskId);

            modelBuilder.Entity<ProcurementData>()
                .HasOne(p => p.Task)
                .WithOne(t => t.ProcurementData)
                .HasForeignKey<ProcurementData>(p => p.TaskId);

            modelBuilder.Entity<DevelopmentData>()
                .HasKey(d => d.TaskId);

            modelBuilder.Entity<DevelopmentData>()
                .HasOne(d => d.Task)
                .WithOne(t => t.DevelopmentData)
                .HasForeignKey<DevelopmentData>(d => d.TaskId);
        }
    }
}
