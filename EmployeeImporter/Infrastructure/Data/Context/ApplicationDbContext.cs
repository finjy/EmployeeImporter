using EmployeeImporter.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeImporter.Infrastructure.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the Employee entity
            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.Surname); // Index for faster sorting by surname

            base.OnModelCreating(modelBuilder);
        }
    }
}
