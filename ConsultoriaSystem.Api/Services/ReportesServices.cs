using ConsultoriaSystem.Api.Dtos;
using ConsultoriaSystem.Api.Repositories;
using static ConsultoriaSystem.Api.Dtos.PageResult;

namespace ConsultoriaSystem.Api.Services
{
    public class ReportesService : IReportesService
    {
        private readonly IReportesRepository _reportesRepository;

        public ReportesService(IReportesRepository reportesRepository)
        {
            _reportesRepository = reportesRepository;
        }

        // /api/v1/reportes/paquetes-por-area
        public async Task<PagedResult<ReportePaquetesPorAreaDTO>> GetPaquetesPorAreaAsync(
            int page,
            int pageSize,
            string? area,
            string? sortBy,
            string? sortDir)
        {
            if (page < 1) page = 1;
            if (pageSize <= 0) pageSize = 10;

            // manda a llamar la data del sp (area, total paquetes)
            var data = await _reportesRepository.GetPaquetesPorAreaAsync();

            // Filtro por area (contiene, case-insensitive)
            if (!string.IsNullOrWhiteSpace(area))
            {
                var areaLower = area.ToLower();
                data = data.Where(x => x.Area.ToLower().Contains(areaLower));
            }

            // Ordenamiento
            sortBy = string.IsNullOrWhiteSpace(sortBy) ? null : sortBy.ToLower();
            sortDir = string.IsNullOrWhiteSpace(sortDir) ? "asc" : sortDir.ToLower();

            IOrderedEnumerable<ReportePaquetesPorAreaDTO>? ordered = null;

            if (sortBy == "area")
                ordered = sortDir == "desc"
                    ? data.OrderByDescending(x => x.Area)
                    : data.OrderBy(x => x.Area);
            else if (sortBy == "totalconsultores")
                ordered = sortDir == "desc"
                    ? data.OrderByDescending(x => x.TotalConsultores)
                    : data.OrderBy(x => x.TotalConsultores);
            else if (sortBy == "totalpaquetes")
                ordered = sortDir == "desc"
                    ? data.OrderByDescending(x => x.TotalPaquetes)
                    : data.OrderBy(x => x.TotalPaquetes);
            else if (sortBy == "totalfacturacion" || sortBy == "totalfacturacionestimada")
                ordered = sortDir == "desc"
                    ? data.OrderByDescending(x => x.TotalFacturacionEstimada)
                    : data.OrderBy(x => x.TotalFacturacionEstimada);

            var query = ordered ?? data; // si no se manda sortBy, respetamos orden del SP

            var totalCount = query.Count();
            var items = query
                .Skip((page - 1) * pageSize)
            .Take(pageSize)
                .ToList();

            return new PagedResult<ReportePaquetesPorAreaDTO>
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = items
            };
        }

        // 🔹 /api/v1/reportes/consultores-top-facturacion
        public async Task<PagedResult<ReporteConsultorTopFacturacionDto>> GetConsultoresTopFacturacionAsync(
            int page,
            int pageSize,
            string? area,
            string? nombre,
            string? sortBy,
            string? sortDir)
        {
            if (page < 1) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var data = await _reportesRepository.GetConsultoresTopFacturacionAsync();

            // Filtro por área
            if (!string.IsNullOrWhiteSpace(area))
            {
                var areaLower = area.ToLower();
                data = data.Where(x => x.AreaEspecializacion.ToLower().Contains(areaLower));
            }

            // Filtro por nombre
            if (!string.IsNullOrWhiteSpace(nombre))
            {
                var nombreLower = nombre.ToLower();
                data = data.Where(x => x.Nombre.ToLower().Contains(nombreLower));
            }

            // Ordenamiento
            sortBy = string.IsNullOrWhiteSpace(sortBy) ? null : sortBy.ToLower();
            sortDir = string.IsNullOrWhiteSpace(sortDir) ? "asc" : sortDir.ToLower();

            IOrderedEnumerable<ReporteConsultorTopFacturacionDto>? ordered = null;

            if (sortBy == "nombre")
                ordered = sortDir == "desc"
                    ? data.OrderByDescending(x => x.Nombre)
                    : data.OrderBy(x => x.Nombre);
            else if (sortBy == "totalpaquetes" || sortBy == "totalpaquetesasignados")
                ordered = sortDir == "desc"
                    ? data.OrderByDescending(x => x.TotalPaquetesAsignados)
                    : data.OrderBy(x => x.TotalPaquetesAsignados);
            else if (sortBy == "totalhoras")
                ordered = sortDir == "desc"
                    ? data.OrderByDescending(x => x.TotalHoras)
                    : data.OrderBy(x => x.TotalHoras);
            else if (sortBy == "totalfacturacion" || sortBy == "totalfacturacionestimada")
                ordered = sortDir == "desc"
                    ? data.OrderByDescending(x => x.TotalFacturacionEstimada)
                    : data.OrderBy(x => x.TotalFacturacionEstimada);

            // Si no se envía sortBy, respetamos el orden del SP (ya viene por facturación desc)
            var query = ordered ?? data;

            var totalCount = query.Count();
            var items = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResult<ReporteConsultorTopFacturacionDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = items
            };
        }
    }
}
