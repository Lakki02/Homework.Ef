using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Data
{
    public class DataDbContext : DbContext 
    {
        public DbSet<Role> Roles { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Preference> Preferences { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<PromoCode> PromoCodes { get; set; }
        public DbSet<CustomerPreference> CustomerPreferences { get; set; }

        public DataDbContext(DbContextOptions<DataDbContext> options) : base(options) { }

        public DataDbContext() { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Role>()
            .Property(r => r.Name)
            .HasMaxLength(100)
            .IsRequired();

            modelBuilder.Entity<Role>()
                .Property(r => r.Description)
                .HasMaxLength(250);

            modelBuilder.Entity<Employee>()
                .Property(e => e.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Employee>()
                .Property(e => e.LastName)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Employee>()
                .Property(e => e.Email)
                .HasMaxLength(200)
                .IsRequired();

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Role)
                .WithMany(r => r.Employees)
                .HasForeignKey(e => e.RoleId);

            modelBuilder.Entity<Preference>()
                .Property(p => p.Name)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Customer>()
                .Property(c => c.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Customer>()
                .Property(c => c.LastName)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Customer>()
                .Property(c => c.Email)
                .HasMaxLength(200)
                .IsRequired();

            modelBuilder.Entity<CustomerPreference>()
                .HasKey(cp => cp.Id);

            modelBuilder.Entity<CustomerPreference>()
                .HasOne(cp => cp.Customer)
                .WithMany(c => c.CustomerPreferences)
                .HasForeignKey(cp => cp.CustomerId);

            modelBuilder.Entity<CustomerPreference>()
                .HasOne(cp => cp.Preference)
                .WithMany(p => p.CustomerPreferences)
                .HasForeignKey(cp => cp.PreferenceId);

            modelBuilder.Entity<PromoCode>()
                .Property(pc => pc.Code)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<PromoCode>()
                .Property(pc => pc.ServiceInfo)
                .HasMaxLength(500);

            modelBuilder.Entity<PromoCode>()
                .Property(pc => pc.PartnerName)
                .HasMaxLength(200)
                .IsRequired();

            modelBuilder.Entity<PromoCode>()
                .HasOne(pc => pc.PartnerManager)
                .WithMany()
                .HasForeignKey(pc => pc.EmployeeId);

            modelBuilder.Entity<PromoCode>()
                .HasOne(pc => pc.Preference)
                .WithMany()
                .HasForeignKey(pc => pc.PreferenceId);

            modelBuilder.Entity<PromoCode>()
                .HasOne(pc => pc.Customer)
                .WithMany(c => c.PromoCodes)
                .HasForeignKey(pc => pc.CustomerId);
        }
    }
}
