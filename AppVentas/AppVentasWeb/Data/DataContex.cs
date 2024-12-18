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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Pais>().HasIndex(p => p.Nombre).IsUnique();
            modelBuilder.Entity<Categoria>().HasIndex(p => p.Nombre).IsUnique();
        }
    }
}