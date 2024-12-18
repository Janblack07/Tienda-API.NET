using API_TIENDA.Models;
using Microsoft.EntityFrameworkCore;

namespace API_TIENDA.Seeders
{
    public static class RoleSeeder
    {
        public static void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rol>().HasData(
                new Rol { Id = 1, Nombre = "Administrador" },
                new Rol { Id = 2, Nombre = "Empleado" },
                new Rol { Id = 3, Nombre = "Cliente" }
            );
        }
    }
}
