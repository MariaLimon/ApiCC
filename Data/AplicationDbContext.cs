// Data/ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;
using ApiCC.Models;

namespace ApiCC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.IsAdmin)
                    .HasDefaultValue(false);

                entity.Property(e => e.IsEmailConfirmed)
                    .HasDefaultValue(false);

                entity.HasIndex(e => e.Email).IsUnique();

                entity.HasData(
                    new User
                    {
                        Id = 1,
                        Name = "Juan",
                        LastName = "Pérez",
                        Email = "juan.perez@example.com",
                        IsAdmin = false,
                        IsEmailConfirmed = true
                    },
                    new User
                    {
                        Id = 2,
                        Name = "María",
                        LastName = "Gómez",
                        Email = "maria.gomez@example.com",
                        IsAdmin = true,
                        IsEmailConfirmed = true
                    }
                );
            });
        }
    }
}