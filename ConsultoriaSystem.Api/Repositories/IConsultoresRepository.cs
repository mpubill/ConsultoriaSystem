using ConsultoriaSystem.Api.Entities;

namespace ConsultoriaSystem.Api.Repositories
{
    public interface IConsultoresRepository
    {
        Task<int> InsertAsync(Consultor consultor);
        Task UpdateAsync(Consultor consultor);
        Task<int> DeleteAsync(int consultorId);
        Task<Consultor?> GetByIdAsync(int consultorId);
        Task<IEnumerable<Consultor>> GetAllAsync();

    }
}
