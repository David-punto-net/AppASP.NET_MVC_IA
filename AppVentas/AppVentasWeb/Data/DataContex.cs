using AppVentasWeb.Data.Entidades;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppVentasWeb.Data
{
    public class DataContex : IdentityDbContext<User>
    {
        public DataContex(DbContextOptions<DataContex> options) : base(options)
        {
        }

        public DbSet<Pais> Paises { get; set; }
        public DbSet<Region> Regiones { get; set; }
        public DbSet<Comuna> Comunas { get; set; }
        public DbSet<Ciudad> Ciudades { get; set; }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
          
            modelBuilder.Entity<Pais>().HasIndex(p => p.Nombre).IsUnique();
            modelBuilder.Entity<Region>().HasIndex("Nombre", "PaisId").IsUnique();
            modelBuilder.Entity<Comuna>().HasIndex("Nombre", "RegionId").IsUnique();
            modelBuilder.Entity<Ciudad>().HasIndex("Nombre", "ComunaId").IsUnique();

            modelBuilder.Entity<Categoria>().HasIndex(p => p.Nombre).IsUnique();
            modelBuilder.Entity<Producto>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<ProductCategory>().HasIndex("ProductoId", "CategoriaId").IsUnique();
        }
    }
}