namespace ConsultoriaSystem.Api.Dtos
{
    public class LogginResponseDTO
    {
        public string Token { get; set; } = null!;
        public DateTime Expiration { get; set; }
    }
}
