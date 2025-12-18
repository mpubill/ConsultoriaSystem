using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ConsultoriaSystem.Api.Dtos;
using ConsultoriaSystem.Api.Entities;
using ConsultoriaSystem.Api.Repositories;

namespace ConsultoriaSystem.Api.Services
{
    public interface IAuthService
    {
        Task<LogginResponseDTO?> LoginAsync(UsuariosDTO request);
    }

    public class AuthService : IAuthService
    {
        private readonly IUsuariosRepository _usuarioRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUsuariosRepository usuarioRepository, IConfiguration configuration)
        {
            _usuarioRepository = usuarioRepository;
            _configuration = configuration;
        }

        public async Task<LogginResponseDTO?> LoginAsync(UsuariosDTO request)
        {
            //Validar credenciales contra la bd usando el sp
            Usuarios? usuario = await _usuarioRepository.LoginAsync(request.Email, request.Password);

            if (usuario == null)
                return null;

            //Leer configuración JWT
            var jwtSection = _configuration.GetSection("Jwt");
            string key = jwtSection["Key"]!;
            string issuer = jwtSection["Issuer"]!;
            string audience = jwtSection["Audience"]!;
            int expirationMinutes = int.Parse(jwtSection["ExpirationMinutes"]!);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Claims
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, usuario.UsuarioId.ToString()),
                new Claim(ClaimTypes.Role, usuario.Rol) };

            // Crear token
            var expires = DateTime.UtcNow.AddMinutes(expirationMinutes);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expires,
                signingCredentials: credentials
            );

            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new LogginResponseDTO
            {
                Token = tokenString,
                Expiration = expires
            };
        }
    }
}
