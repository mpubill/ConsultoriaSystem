namespace ConsultoriaSystem.Api.Dtos
{
    public class ConsultorCreateRequest
    {
        public string Nombre { get; set; } = null!;
        public string AreaEspecializacion { get; set; } = null!;
        public decimal TarifaHora { get; set; }
        public string EmailCorporativo { get; set; } = null!;
    }

    public class ConsultorUpdateRequest
    {
        public string Nombre { get; set; } = null!;
        public string AreaEspecializacion { get; set; } = null!;
        public decimal TarifaHora { get; set; }
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


