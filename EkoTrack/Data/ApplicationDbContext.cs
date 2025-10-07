using EkoTrack.Models;
using Microsoft.EntityFrameworkCore;

namespace EkoTrack.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<AppUser> Users => Set<AppUser>();
        public DbSet<Material> Materials => Set<Material>();
        public DbSet<RecyclingRecord> RecyclingRecords => Set<RecyclingRecord>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUser>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Semillas de materiales comunes (puedes editar)
            modelBuilder.Entity<Material>().HasData(
                new Material { Id = 1, Nombre = "Plástico", Unidad = "kg" },
                new Material { Id = 2, Nombre = "Vidrio", Unidad = "kg" },
                new Material { Id = 3, Nombre = "Cartón", Unidad = "kg" },
                new Material { Id = 4, Nombre = "Tetrapack", Unidad = "kg" },
                new Material { Id = 5, Nombre = "Batería", Unidad = "kg" }
            );
        }
    }
}

