using ConsultoriaSystem.Api.Dtos;

namespace ConsultoriaSystem.Api.Services
{
    public interface IPaquetesService
    {
        Task<int> CreateAsync(PaqueteDTO dto);
        Task UpdateAsync(PaqueteDTO dto);
        Task<bool> DeleteAsync(int paqueteId);
        Task<PaqueteDTO?> GetByIdAsync(int paqueteId);
        Task<IEnumerable<PaqueteDTO>> GetAllAsync();
    }
}
