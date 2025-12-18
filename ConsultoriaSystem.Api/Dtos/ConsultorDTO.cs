using System.ComponentModel.DataAnnotations;

namespace ConsultoriaSystem.Api.Dtos
{
    public class ConsultorCreateRequest
    {
        [Required]
        [StringLength(150)]
        public string Nombre { get; set; } = null!;

        [Required]
        [StringLength(150)]
        public string AreaEspecializacion { get; set; } = null!;

        [Range(30, 200, ErrorMessage = "La tarifa debe estar entre $30 y $200.")]
        public decimal TarifaHora { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string EmailCorporativo { get; set; } = null!;
    }

    public class ConsultorUpdateRequest
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(150, ErrorMessage = "El nombre debe tener máximo 150 caracteres.")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "El área de especialización es obligatoria.")]
        [StringLength(150, ErrorMessage = "El área debe tener máximo 150 caracteres.")]
        public string AreaEspecializacion { get; set; } = null!;

        [Range(30, 200, ErrorMessage = "La tarifa por hora debe estar entre $30 y $200.")]
        public decimal TarifaHora { get; set; }

        [Required(ErrorMessage = "El email corporativo es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
        [StringLength(150, ErrorMessage = "El email debe tener máximo 150 caracteres.")]
        public string EmailCorporativo { get; set; } = null!;

        public bool Activo { get; set; }
    }

    public class ConsultorDTO
    {
        public int? ConsultorId { get; set; }
        public string Nombre { get; set; } = null!;
        public string AreaEspecializacion { get; set; } = null!;
        public decimal TarifaHora { get; set; }
        public string EmailCorporativo { get; set; } = null!;
        public bool? Activo { get; set; }
        public DateTime FechaIngreso { get; set; }
    }
}