using API_TIENDA.Migrations;
using API_TIENDA.Models;
using Microsoft.EntityFrameworkCore;

namespace API_TIENDA.Servicios
{
    public class DbTienda : DbContext
    {
        public DbTienda(DbContextOptions<DbTienda> options) : base(options)
        {

        }

        public DbSet<Productos> Productos {get ; set;}
        public DbSet<Categoria> Categorias {get ; set;}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relación entre Producto y Categoria
            modelBuilder.Entity<Productos>()
                .HasOne(p => p.Categoria)
                .WithMany(c => c.Productos)
                .HasForeignKey(p => p.CategoriaId);
        }
    }
}
