using System.ComponentModel.DataAnnotations;

namespace ConsultoriaSystem.Api.Entities
{
    public class Consultor
    {
        public int ConsultorId { get; set; }
        public string Nombre { get; set; } = null!;
        public string AreaEspecializacion { get; set; } = null!;
        public decimal TarifaHora { get; set; }
        public string EmailCorporativo { get; set; } = null!;
        public bool Activo { get; set; }
        public DateTime FechaIngreso { get; set; }
    }
}
