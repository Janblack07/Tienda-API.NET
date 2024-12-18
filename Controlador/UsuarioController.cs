using API_TIENDA.Dto;
using API_TIENDA.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
    }
}
