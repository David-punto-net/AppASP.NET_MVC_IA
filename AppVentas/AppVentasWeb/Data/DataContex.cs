using AppVentasWeb.Data.Entidades;
using Microsoft.EntityFrameworkCore;

namespace AppVentasWeb.Data
{
    public class DataContex : DbContext
    {
        public DataContex(DbContextOptions<DataContex> options) : base(options)
        {
        }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Pais> Paises { get; set; }
        public DbSet<Region> Regiones { get; set; }
        public DbSet<Comuna> Comunas { get; set; }
        public DbSet<Ciudad> Ciudades { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Categoria>().HasIndex(p => p.Nombre).IsUnique();
            modelBuilder.Entity<Pais>().HasIndex(p => p.Nombre).IsUnique();

            modelBuilder.Entity<Region>().HasIndex("Nombre", "PaisId").IsUnique();
            modelBuilder.Entity<Comuna>().HasIndex("Nombre", "RegionId").IsUnique();
            modelBuilder.Entity<Ciudad>().HasIndex("Nombre", "ComunaId").IsUnique();
        }
    }
}