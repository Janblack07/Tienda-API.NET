using System.ComponentModel.DataAnnotations;

namespace API_TIENDA.Dto
{
    public class ProductosDto
    {
        public int Id { get; set; }
        public String Nombre { get; set; }
        public double Precio { get; set; }
        public String Descripcion { get; set; }
        public String ImageUrl { get; set; }
        public int CategoriaId { get; set; }  // Relación con la categoría
    }
    public class CreateProductoDto
    {
        public string Nombre { get; set; }
        public double Precio { get; set; }
        public string Descripcion { get; set; }
        public IFormFile Imagen { get; set; } // La imagen será recibida como un archivo
        public int CategoriaId { get; set; } // Se debe proporcionar el Id de la categoría
    }
    public class UpdateProductoDto
    {
        public string Nombre { get; set; }
        public double? Precio { get; set; } // Puede ser null si no se desea actualizar
        public string Descripcion { get; set; }
        public IFormFile Imagen { get; set; } // La imagen será recibida como un archivo
        public int CategoriaId { get; set; } // Se debe proporcionar el Id de la categoría
    }
}
