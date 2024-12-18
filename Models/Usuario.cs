namespace API_TIENDA.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }

        // Relación con el Rol
        public int RolId { get; set; }
        public Rol Rol { get; set; }
    }
}