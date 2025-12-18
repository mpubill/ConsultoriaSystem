using ConsultoriaSystem.Api.Entities;

namespace ConsultoriaSystem.Api.Repositories
{
    public interface IPaquetesRepository
    {
        Task<int> InsertAsync(PaqueteServicio paquete);
        Task UpdateAsync(PaqueteServicio paquete);
        Task DeleteAsync(int paqueteId);
        Task<PaqueteServicio?> GetByIdAsync(int paqueteId);
        Task<IEnumerable<PaqueteServicio>> GetAllAsync();
    }
}
