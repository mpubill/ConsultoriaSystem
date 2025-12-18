using System.ComponentModel.DataAnnotations;

namespace ConsultoriaSystem.Api.Entities
{
    public class Usuarios
    {
        public int UsuarioId { get; set; }
        public string Email { get; set; } = null!;
        public string Rol { get; set; } = null!;
    }
}
