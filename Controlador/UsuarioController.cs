using API_TIENDA.Dto;
using API_TIENDA.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_TIENDA.Controlador
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly DbTienda _context;

        public UsuarioController(AuthService authService, DbTienda context)
        {
            _authService = authService;
            _context = context;
        }

        [HttpPost]
        [Route("RegisterCliente")]
        public async Task<IActionResult> RegisterCliente([FromBody] RegistroDto dto)
        {
            try
            {
                dto.Rol = "Cliente"; // Asignar rol fijo de cliente
                var usuario = await _authService.Register(dto);
                return Ok(new { message = "Cliente registrado exitosamente.", usuario });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Registro de EMPLEADOS (solo autorizado por Administrador)
        [HttpPost]
        [Route("RegisterEmpleado")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> RegisterEmpleado([FromBody] RegistroDto dto)
        {
            try
            {
                if (dto.Rol != "Empleado")
                    return BadRequest(new { message = "El rol debe ser 'Empleado' para este registro." });

                var usuario = await _authService.Register(dto);
                return Ok(new { message = "Empleado registrado exitosamente.", usuario });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                var token = await _authService.Login(dto);
                return Ok(new {message = "Usted a iniciado Sesion : ", token });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }


        // Obtener perfil del usuario autenticado
        [HttpGet]
        [Route("Perfil")]
        [Authorize] // Solo usuarios autenticados
        public async Task<IActionResult> GetPerfil()
        {
            var userId = int.Parse(User.Identity.Name); // Extraer el ID desde el token
            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (usuario == null) return NotFound(new { message = "Usuario no encontrado." });

            var perfilDto = new UsuarioDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Direccion = usuario.Direccion,
                Telefono = usuario.Telefono,
                Rol = usuario.Rol.Nombre
            };

            return Ok(perfilDto);
        }

        // Listar empleados (solo para Administradores)
        [HttpGet]
        [Route("ListEmpleados")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetEmpleados()
        {
            var empleados = await _context.Usuarios
                .Include(u => u.Rol)
                .Where(u => u.Rol.Nombre == "Empleado")
                .ToListAsync();

            var result = empleados.Select(e => new UsuarioDto
            {
                Id = e.Id,
                Nombre = e.Nombre,
                Email = e.Email,
                Direccion = e.Direccion,
                Telefono = e.Telefono,
                Rol = e.Rol.Nombre
            });

            return Ok(result);
        }

        // Listar clientes (solo para Administradores y Empleados)
        [HttpGet]
        [Route("ListClientes")]
        [Authorize(Roles = "Administrador,Empleado")]
        public async Task<IActionResult> GetClientes()
        {
            var clientes = await _context.Usuarios
                .Include(u => u.Rol)
                .Where(u => u.Rol.Nombre == "Cliente")
                .ToListAsync();

            var result = clientes.Select(c => new UsuarioDto
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Email = c.Email,
                Direccion = c.Direccion,
                Telefono = c.Telefono,
                Rol = c.Rol.Nombre
            });

            return Ok(result);
        }
    }
}
