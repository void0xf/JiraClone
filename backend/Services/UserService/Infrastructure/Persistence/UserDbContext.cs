using Microsoft.EntityFrameworkCore;
using UserService.Models;

namespace UserService.Infrastructure;

public class UserDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id); // Local DB Primary Key

            // --- Add Unique Index on KeycloakUserId ---
            entity.HasIndex(u => u.KeycloakUserId)
                .IsUnique(); // Ensure only one profile per Keycloak user

            // --- Existing Configuration ---
            // Configure how FullName (Value Object) is stored
            // Assuming PrivacySetting is an owned entity type or complex property
            entity.OwnsOne(u => u.FullName, fn =>
            {
                fn.Property(p => p.Value).HasColumnName("FullNameValue").HasMaxLength(100);
                fn.Property(p => p.WhoCanSee).HasColumnName("FullNamePrivacy");
                // Make the value required if the FullName object itself is required
                fn.Property(p => p.Value).IsRequired();
            });

            entity.OwnsOne(u => u.PublicName, pn =>
            {
                pn.Property(p => p.Value).HasColumnName("PublicNameValue").HasMaxLength(100);
                pn.Property(p => p.WhoCanSee).HasColumnName("PublicNamePrivacy");
                // PublicName might be optional, so Value might not be required
            });

            // Ensure required properties are marked
            entity.Property(u => u.Email).IsRequired();
            entity.Property(u => u.KeycloakUserId).IsRequired();


        });
    }

}