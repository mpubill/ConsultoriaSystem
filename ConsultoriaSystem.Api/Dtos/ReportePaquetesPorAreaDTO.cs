namespace ConsultoriaSystem.Api.Dtos
{
    public class ReportePaquetesPorAreaDTO
    {
        public string Area { get; set; } = null!;
        public int TotalConsultores { get; set; }
        public int TotalPaquetes { get; set; }
        public decimal TotalFacturacionEstimada { get; set; }
    }
}

