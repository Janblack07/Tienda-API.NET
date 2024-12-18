using API_TIENDA.Models;
using Microsoft.EntityFrameworkCore;

namespace API_TIENDA.Seeders
{
    public static class UserSeeder
    {
        public static void SeedAdminUser(ModelBuilder modelBuilder)
        {
            var password = BCrypt.Net.BCrypt.HashPassword("Admin123");

            modelBuilder.Entity<Usuario>().HasData(
                new Usuario
                {
                    Id = 1,
                    Nombre = "Admin",
                    Email = "admin@tienda.com",
                    Password = password,
                    Direccion = "123 Admin St",
                    Telefono = "123456789",
                    RolId = 1 // Rol Administrador
                }
            );
        }
    }
}
