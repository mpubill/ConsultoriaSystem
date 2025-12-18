using System.ComponentModel.DataAnnotations;

namespace ConsultoriaSystem.Api.Dtos
{
    public class UsuariosDTO
    {
        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El email no tiene un formato válido.")]
        [StringLength(100, ErrorMessage = "El email no debe superar los 100 caracteres.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(100, MinimumLength = 6,
        ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        public string Password { get; set; } = null!;
    }
}
