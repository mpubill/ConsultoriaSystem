using System.ComponentModel.DataAnnotations;

namespace ConsultoriaSystem.Api.Dtos
{
    public class PaqueteCreateRequest
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres.")]
        public string Nombre { get; set; } = null!;

        [Range(1, 200, ErrorMessage = "La duración debe ser entre 1 y 200 horas.")]
        public int DuracionHoras { get; set; }

        [Range(1, 999999, ErrorMessage = "El costo debe ser mayor a 0.")]
        public decimal Costo { get; set; }

        [StringLength(500, ErrorMessage = "Máximo 500 caracteres.")]
        public string? Descripcion { get; set; }
    }

    public class PaqueteUpdateRequest
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100)]
        public string Nombre { get; set; } = null!;

        [Range(1, 200)]
        public int DuracionHoras { get; set; }

        [Range(1, 999999)]
        public decimal Costo { get; set; }

        [StringLength(500)]
        public string? Descripcion { get; set; }

        public bool Activo { get; set; }
    }

    public class PaqueteDTO //Para respuestas, con todo los campos (Salida)
    {
        public int? PaqueteId { get; set; }
        public string Nombre { get; set; } = null!;
        public int DuracionHoras { get; set; }
        public decimal Costo { get; set; }
        public string? Descripcion { get; set; }
        public bool? Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}