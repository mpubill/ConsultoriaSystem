using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ConsultoriaSystem.Api.Services;

namespace ConsultoriaSystem.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize] // todos los roles: Admin y User
    public class ReportesController : ControllerBase
    {
        private readonly IReportesService _reportesService;

        public ReportesController(IReportesService reportesService)
        {
            _reportesService = reportesService;
        }

        // GET /api/v1/reportes/paquetes-por-area
        [HttpGet("paquetes-por-area")]
        public async Task<IActionResult> GetPaquetesPorArea(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? area = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] string? sortDir = "asc")
        {
            var result = await _reportesService.GetPaquetesPorAreaAsync(page, pageSize, area, sortBy, sortDir);
            return Ok(result);
        }

        // GET /api/v1/reportes/consultores-top-facturacion
        [HttpGet("consultores-top-facturacion")]
        public async Task<IActionResult> GetConsultoresTopFacturacion(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? area = null,
            [FromQuery] string? nombre = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] string? sortDir = "asc")
        {
            var result = await _reportesService.GetConsultoresTopFacturacionAsync(
                page, pageSize, area, nombre, sortBy, sortDir);

            return Ok(result);
        }
    }
}

