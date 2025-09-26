using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GestionOrdenes.DTOs;
using GestionOrdenes.Interfaces;
using GestionOrdenes.Models;

namespace GestionOrdenes.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly List<User> _users; // En producción esto debería venir de una base de datos

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
            // Usuarios de prueba - en producción esto debería venir de la base de datos
            _users = new List<User>
            {
                new User { Id = 1, Username = "admin", Password = "admin123", Role = "Admin" },
                new User { Id = 2, Username = "user", Password = "user123", Role = "User" }
            };
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto loginDto)
        {
            var isValid = await ValidateUserAsync(loginDto.Username, loginDto.Password);
            if (!isValid)
            {
                return null;
            }

            var user = _users.FirstOrDefault(u => u.Username == loginDto.Username);
            if (user == null)
            {
                return null;
            }

            var token = GenerateJwtToken(user.Username, user.Role);
            var expiration = DateTime.UtcNow.AddHours(24);

            return new AuthResponseDto
            {
                Token = token,
                Expiration = expiration,
                Username = user.Username,
                Role = user.Role
            };
        }

        public async Task<bool> ValidateUserAsync(string username, string password)
        {
            // En producción, aquí deberías verificar contra la base de datos
            // y comparar el hash de la contraseña
            var user = _users.FirstOrDefault(u => u.Username == username && u.Password == password);
            return await Task.FromResult(user != null);
        }

        public string GenerateJwtToken(string username, string role)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"] ?? "MySecretKey12345678901234567890");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, role),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
                }),
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = jwtSettings["Issuer"] ?? "GestionOrdenesAPI",
                Audience = jwtSettings["Audience"] ?? "GestionOrdenesAPI"
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}