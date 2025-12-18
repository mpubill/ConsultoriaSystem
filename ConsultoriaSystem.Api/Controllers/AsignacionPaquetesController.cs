using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ConsultoriaSystem.Api.Services;
using ConsultoriaSystem.Api.Common;

namespace ConsultoriaSystem.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize] // todos requieren token 
    public class AsignacionPaquetesController : ControllerBase
    {
        private readonly IConsultorPaqueteService _consultorPaqueteService;

        public AsignacionPaquetesController(IConsultorPaqueteService consultorPaqueteService)
        {
            _consultorPaqueteService = consultorPaqueteService;
        }

        // POST /api/v1/consultores/{id}/paquetes/{paqueteId} (asignar paquete)
        [HttpPost("{id:int}/paquetes/{paqueteId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AsignarPaquete(int id, int paqueteId)
        {
            var asignacionId = await _consultorPaqueteService.AsignarPaqueteAsync(id, paqueteId);

            var response = ApiResponse<object>.SuccessResponse(
                data: new { asignacionId },
                message: "Paquete asignado al consultor correctamente.",
                statusCode: StatusCodes.Status200OK);

            return Ok(response);
        }

        // DELETE /api/v1/consultores/{id}/paquetes/{paqueteId} (desasignar paquete)
        [HttpDelete("{id:int}/paquetes/{paqueteId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DesasignarPaquete(int id, int paqueteId)
        {
            await _consultorPaqueteService.DesasignarPaqueteAsync(id, paqueteId);

            var response = ApiResponse.SuccessResponse(
                message: "Paquete desasignado del consultor correctamente.",
                statusCode: StatusCodes.Status200OK);

            return Ok(response);
        }
    }
}
