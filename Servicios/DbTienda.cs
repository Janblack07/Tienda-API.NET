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
}
}
