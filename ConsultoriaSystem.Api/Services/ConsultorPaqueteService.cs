using System.Threading.Tasks;
using ConsultoriaSystem.Api.Repositories;

namespace ConsultoriaSystem.Api.Services
{
    public class ConsultorPaqueteService : IConsultorPaqueteService
    {
        private readonly IConsultorPaqueteRepository _repository;

        public ConsultorPaqueteService(IConsultorPaqueteRepository repository)
        {
            _repository = repository;
        }

        public Task<int> AsignarPaqueteAsync(int consultorId, int paqueteId)
        {
            return _repository.AsignarPaqueteAsync(consultorId, paqueteId);
        }

        public Task DesasignarPaqueteAsync(int consultorId, int paqueteId)
        {
            return _repository.DesasignarPaqueteAsync(consultorId, paqueteId);
        }
    }
}
