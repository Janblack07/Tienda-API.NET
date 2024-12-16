using API_TIENDA.Migrations;
using System.ComponentModel.DataAnnotations;

namespace API_TIENDA.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public String Nombre { get; set; }
        [MaxLength(100)]
        public String Descripcion { get; set; }
        public String ImageUrl { get; set; } // Para almacenar la URL de la imagen
                                             // Relación con los productos (un producto pertenece a una categoría)
        public ICollection<Productos> Productos { get; set; }
    }
}
