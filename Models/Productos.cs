using System.ComponentModel.DataAnnotations;

namespace API_TIENDA.Models
{
    public class Productos
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public String Nombre { get; set;}
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que 0")]
        public double Precio { get; set; }
        [MaxLength(100)]
        public String Descripcion { get; set; }

        public String ImageUrl { get; set; } // Para almacenar la URL de la imagen

        // Relación con la categoría
        public int CategoriaId { get; set; }  // Clave foránea
        public Categoria Categoria { get; set; }  // Relación con la entidad Categoria

    }
}
