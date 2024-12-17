using AppVentasWeb.Data.Entidades;
using Microsoft.EntityFrameworkCore;

namespace AppVentasWeb.Data
{
    public class DataContex : DbContext
    {
        public DataContex(DbContextOptions<DataContex> options ): base(options) 
        {
            
        }

        public DbSet<Pais> Paises { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Pais>().HasIndex(p => p.Nombre).IsUnique();
        }
    }
}
