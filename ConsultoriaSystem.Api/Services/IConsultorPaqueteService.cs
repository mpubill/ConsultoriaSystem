using System.Threading.Tasks;

namespace ConsultoriaSystem.Api.Services
{
    public interface IConsultorPaqueteService
    {
        Task<int> AsignarPaqueteAsync(int consultorId, int paqueteId);
        Task DesasignarPaqueteAsync(int consultorId, int paqueteId);
    }
}
