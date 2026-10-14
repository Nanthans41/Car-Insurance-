using CarInsurance.Models;
using Microsoft.EntityFrameworkCore;

namespace CarInsurance.Data
{
    // Manages the connection between the application and the database.
    public class InsuranceContext : DbContext
    {
        // Receives the database configuration options.
        public InsuranceContext(
            DbContextOptions<InsuranceContext> options)
            : base(options)
        {
        }

        // Represents the Insurees table in the database.
        public DbSet<Insuree> Insurees { get; set; } = null!;
       
        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Store insurance quotes as decimal values with two
            // digits after the decimal point.
            modelBuilder.Entity<Insuree>()
                .Property(i => i.Quote)
                .HasPrecision(10, 2);
        }
    }
}