using System.Threading.Tasks;

namespace ConsultoriaSystem.Api.Repositories
{
    public interface IConsultorPaqueteRepository
    {
        Task<int> AsignarPaqueteAsync(int consultorId, int paqueteId);
        Task DesasignarPaqueteAsync(int consultorId, int paqueteId);
    }
}
