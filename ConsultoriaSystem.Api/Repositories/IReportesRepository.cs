
using ConsultoriaSystem.Api.Dtos;

namespace ConsultoriaSystem.Api.Repositories
{
    public interface IReportesRepository
    {
        Task<IEnumerable<ReportePaquetesPorAreaDTO>> GetPaquetesPorAreaAsync();
        Task<IEnumerable<ReporteConsultorTopFacturacionDto>> GetConsultoresTopFacturacionAsync();
    }
}

