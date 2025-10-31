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

            // Configuración específica para PostgreSQL
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

                // Índice único para email
                entity.HasIndex(e => e.Email).IsUnique();

                entity.Property(e => e.IsEmailConfirmed)
                    .IsRequired()
                    .HasDefaultValue(false);

                entity.Property(e => e.IsAdmin)
                    .IsRequired()
                    .HasDefaultValue(false);

                // Datos iniciales
                entity.HasData(
                    new User
                    {
                        Id = 1,
                        Name = "Juan",
                        LastName = "Pérez",
                        Email = "juan.perez@ejemplo.com",
                        IsEmailConfirmed = false,
                        IsAdmin = false
                    },
                    new User
                    {
                        Id = 2,
                        Name = "María",
                        LastName = "Gómez",
                        Email = "maria.gomez@ejemplo.com",
                        IsEmailConfirmed = false,
                        IsAdmin = false
                    }
                );
            });
        }
    }
}