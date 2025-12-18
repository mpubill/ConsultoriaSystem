using ConsultoriaSystem.Api.Dtos;
using static ConsultoriaSystem.Api.Dtos.PageResult;

namespace ConsultoriaSystem.Api.Services
{
    public interface IReportesService
    {
        Task<PagedResult<ReportePaquetesPorAreaDTO>> GetPaquetesPorAreaAsync(
            int page,
            int pageSize,
            string? area,
            string? sortBy,
            string? sortDir);

        Task<PagedResult<ReporteConsultorTopFacturacionDto>> GetConsultoresTopFacturacionAsync(
            int page,
            int pageSize,
            string? area,
            string? nombre,
            string? sortBy,
            string? sortDir);
    }
}

