using ConsultoriaSystem.Api.Dtos;

namespace ConsultoriaSystem.Api.Services
{
    public interface IConsultoresService
    {
        Task<int> CreateAsync(ConsultorDTO dto);
        Task UpdateAsync(ConsultorDTO dto);
        Task<bool> DeleteAsync(int consultorId);
        Task<ConsultorDTO?> GetByIdAsync(int consultorId);
        Task<IEnumerable<ConsultorDTO>> GetAllAsync();
    }
}

