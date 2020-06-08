using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Entities
{
    public class RepositoryContext: 
        IdentityUserContext<ApplicationUser, int, IdentityUserClaim<int>, IdentityUserLogin<int>, IdentityUserToken<int>>
    {
        public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Address>()
                .HasOne(address => address.Landlord)
                .WithOne(landlord => landlord.Address)
                .HasForeignKey<Landlord>(landlord => landlord.AddressId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("AddressId");

            modelBuilder.Entity<Address>()
                .HasOne(address => address.Property)
                .WithOne(property => property.Address)
                .HasForeignKey<Property>(property => property.AddressId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("AddressId");

            modelBuilder.Entity<Address>()
                .HasOne(address => address.Tenant)
                .WithOne(tenant => tenant.Address)
                .HasForeignKey<Tenant>(tenant => tenant.AddressId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("AddressId");

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(a => a.Tenant)
                .WithOne(t => t.ApplicationUser)
                .HasForeignKey<Tenant>(t => t.AspNetUsersId)
                .OnDelete(DeleteBehavior.NoAction);
  
            modelBuilder.Entity<ApplicationUser>()
                .HasOne(a => a.Landlord)
                .WithOne(l => l.ApplicationUser)
                .HasForeignKey<Landlord>(l => l.AspNetUsersId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Property>()
                .HasOne(property => property.Landlord)
                .WithMany(landlord => landlord.Properties)
                .HasForeignKey(property => property.LandlordId);

            modelBuilder.Entity<Rent>()
                .HasOne(rent => rent.Property)
                .WithMany(property => property.Rents)
                .HasForeignKey(rent => rent.PropertyId);

            modelBuilder.Entity<Rent>()
                .HasOne(rent => rent.Tenant)
                .WithMany(tenant => tenant.Rents)
                .HasForeignKey(rent => rent.TenantId);

            modelBuilder.Entity<Rent>()
                .HasOne(rent => rent.Landlord)
                .WithMany(landlord => landlord.Rents)
                .HasForeignKey(rent => rent.LandlordId);

            modelBuilder.Entity<Rate>()
                .HasOne(rate => rate.Property)
                .WithOne(property => property.Rate)
                .HasForeignKey<Rate>(rate => rate.PropertyId);

            modelBuilder.Entity<State>()
                .HasOne(state => state.Rent)
                .WithMany(rent => rent.States)
                .HasForeignKey(state => state.RentId);

            modelBuilder.Entity<Invoice>()
                .HasOne(invoice => invoice.Rent)
                .WithMany(rent => rent.Invoices)
                .HasForeignKey(invoice => invoice.RentId);

            modelBuilder.Entity<Invoice>()
                .HasOne(invoice => invoice.State)
                .WithOne(state => state.Invoice)
                .HasForeignKey<Invoice>(invoice => invoice.StateId);

            modelBuilder.Entity<Photo>()
                .HasOne(photo => photo.State)
                .WithOne(state => state.Photo)
                .HasForeignKey<Photo>(photo => photo.StateId);

        }

        public DbSet<Landlord> Landlords { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<Rent> Rents { get; set; }
        public DbSet<State> State { get; set; }
        public DbSet<ApplicationUser> applicationUsers { get; set; }

    }
}
