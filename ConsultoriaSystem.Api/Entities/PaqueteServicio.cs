using System.ComponentModel.DataAnnotations;

namespace ConsultoriaSystem.Api.Entities
{
    public class PaqueteServicio
    {
        public int PaqueteId { get; set; }
        public string Nombre { get; set; } = null!;
        public int DuracionHoras { get; set; }
        public decimal Costo { get; set; }
        public string? Descripcion { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
