using API_TIENDA.Migrations;
using API_TIENDA.Models;
using API_TIENDA.Seeders;
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
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relación entre Producto y Categoria
            modelBuilder.Entity<Productos>()
                .HasOne(p => p.Categoria)
                .WithMany(c => c.Productos)
                .HasForeignKey(p => p.CategoriaId);
            // Relación entre Usuario y Rol (Un rol puede tener muchos usuarios)
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Rol)
                .WithMany(r => r.Usuarios)
                .HasForeignKey(u => u.RolId);

            // Configuración adicional de claves únicas
            modelBuilder.Entity<Rol>()
                .HasIndex(r => r.Nombre)
                .IsUnique(); // Garantiza que el nombre del rol sea único

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique(); // Garantiza que el email sea único

            // Llamada al Seeder de Roles
            RoleSeeder.SeedRoles(modelBuilder);
            // Llamada al Seeder de Usuarios
            UserSeeder.SeedAdminUser(modelBuilder);
        }
    }
}
