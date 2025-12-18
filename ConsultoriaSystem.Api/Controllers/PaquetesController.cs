using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ConsultoriaSystem.Api.Services;
using ConsultoriaSystem.Api.Dtos;
using ConsultoriaSystem.Api.Common;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace ConsultoriaSystem.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class PaquetesController : ControllerBase
    {
        private readonly IPaquetesService _paquetesService;

        public PaquetesController(IPaquetesService paquetesService)
        {
            _paquetesService = paquetesService;
        }

        // GET /api/v1/paquetes
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _paquetesService.GetAllAsync();

            var response = ApiResponse<IEnumerable<PaqueteDTO>>.SuccessResponse(
                data: result,
                message: "Paquetes obtenidos correctamente.",
                statusCode: StatusCodes.Status200OK
            );

            return Ok(response);
        }

        // GET /api/v1/paquetes/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var paquete = await _paquetesService.GetByIdAsync(id);
            if (paquete == null)
            {
                var notFoundResponse = ApiResponse<PaqueteDTO>.ErrorResponse(
                    message: "Paquete no encontrado.",
                    statusCode: StatusCodes.Status404NotFound
                );

                return NotFound(notFoundResponse);
            }

            var response = ApiResponse<PaqueteDTO>.SuccessResponse(
                data: paquete,
                message: "Paquete obtenido correctamente.",
                statusCode: StatusCodes.Status200OK
            );

            return Ok(response);
        }

        // POST /api/v1/paquetes  (solo Admin)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] PaqueteCreateRequest request)
        {

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                var errorResponse = ApiResponse<object>.ErrorResponse(
                    message: "Errores de validación.",
                    statusCode: StatusCodes.Status400BadRequest,
                    errors: errors
                );

                return StatusCode(errorResponse.StatusCode, errorResponse);
            }

            var dto = new PaqueteDTO
            {
                Nombre = request.Nombre,
                DuracionHoras = request.DuracionHoras,
                Costo = request.Costo,
                Descripcion = request.Descripcion,
                Activo = true // típico por defecto
                // Id y FechaCreacion los pone la BD
            };

            var id = await _paquetesService.CreateAsync(dto);

            var response = ApiResponse<object>.SuccessResponse(
                data: new { id },
                message: "Paquete creado correctamente.",
                statusCode: StatusCodes.Status201Created
            );

            return StatusCode(StatusCodes.Status201Created, response);
        }

        // PUT /api/v1/paquetes/{id}  (solo Admin)
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] PaqueteUpdateRequest request)
        {

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                var errorResponse = ApiResponse.ErrorResponse(
                    message: "Errores de validación.",
                    statusCode: StatusCodes.Status400BadRequest,
                    errors: errors
                );

                return StatusCode(errorResponse.StatusCode, errorResponse);
            }

            var dto = new PaqueteDTO
            {
                PaqueteId = id,
                Nombre = request.Nombre,
                DuracionHoras = request.DuracionHoras,
                Costo = request.Costo,
                Descripcion = request.Descripcion,
                Activo = request.Activo
            };

            await _paquetesService.UpdateAsync(dto);

            var response = ApiResponse.SuccessResponse(
                message: "Paquete actualizado correctamente.",
                statusCode: StatusCodes.Status200OK
            );

            return Ok(response);
        }

        // DELETE /api/v1/paquetes/{id} (solo Admin)
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _paquetesService.DeleteAsync(id);

            if (!deleted)
            {
                var notFound = ApiResponse.ErrorResponse(
                    message: "Paquete no encontrado.",
                    statusCode: StatusCodes.Status404NotFound
                );

                return NotFound(notFound);
            }

            var ok = ApiResponse.SuccessResponse(
                message: "Paquete eliminado correctamente.",
                statusCode: StatusCodes.Status200OK
            );

            return Ok(ok);
        }
    }
}