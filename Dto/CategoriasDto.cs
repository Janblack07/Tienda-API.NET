namespace API_TIENDA.Dto
{
    public class CategoriasDto
    {
        public int Id { get; set; }
        public String Nombre { get; set; }
        public String Descripcion { get; set; }
        public String ImageUrl { get; set; }
    }
    public class CreateCategoriaDto
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public IFormFile Imagen { get; set; } // La imagen será recibida como un archivo
    }
    public class UpdateCategoriaDto
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public IFormFile Imagen { get; set; } // La imagen será recibida como un archivo
    }
}
