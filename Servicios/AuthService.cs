﻿using API_TIENDA.Dto;
using API_TIENDA.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API_TIENDA.Servicios
{
    public class AuthService
    {
        private readonly DbTienda _context;
        private readonly IConfiguration _config;

        public AuthService(DbTienda context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        // Registro
        public async Task<Usuario> Register(RegistroDto dto)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Nombre == dto.Rol);
            if (role == null) throw new Exception("Rol no válido.");

            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Direccion = dto.Direccion,
                Telefono = dto.Telefono,
                RolId = role.Id
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        // Login
        public async Task<string> Login(LoginDto dto)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(dto.Password, usuario.Password))
                throw new Exception("Credenciales incorrectas.");

            // Generar el token JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Secret"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, usuario.Id.ToString()),
                new Claim(ClaimTypes.Role, usuario.Rol.Nombre)
            }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
