namespace API_TIENDA.Dto
{
    public class RegistroDto
    {
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Contraseña { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public int Rol { get; set; } // "Administrador", "Empleado", "Cliente"
    }
}
