namespace ConsultoriaSystem.Api.Dtos
{
    public class ReporteConsultorTopFacturacionDto
    {
        public int ConsultorId { get; set; }
        public string Nombre { get; set; } = null!;
        public string AreaEspecializacion { get; set; } = null!;
        public string EmailCorporativo { get; set; } = null!;
        public int TotalPaquetesAsignados { get; set; }
        public int TotalHoras { get; set; }
        public decimal TotalFacturacionEstimada { get; set; }
    }
}
