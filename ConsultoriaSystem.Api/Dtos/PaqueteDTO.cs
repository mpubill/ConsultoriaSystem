namespace ConsultoriaSystem.Api.Dtos
{
    public class PaqueteCreateRequest
    {
        public string Nombre { get; set; } = null!;
        public int DuracionHoras { get; set; }
        public decimal Costo { get; set; }
        public string? Descripcion { get; set; }
    }

    public class PaqueteUpdateRequest
    {
        public string Nombre { get; set; } = null!;
        public int DuracionHoras { get; set; }
        public decimal Costo { get; set; }
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
