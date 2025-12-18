using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ConsultoriaSystem.Api.Services;
using ConsultoriaSystem.Api.Dtos;
using ConsultoriaSystem.Api.Common;  

namespace ConsultoriaSystem.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize] // todos requieren token 
    public class ConsultoresController : ControllerBase
    {
        private readonly IConsultoresService _consultoresService;

        public ConsultoresController(IConsultoresService consultoresService)
        {
            _consultoresService = consultoresService;
        }

        // GET /api/v1/consultores
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _consultoresService.GetAllAsync(); 

            var response = ApiResponse<IEnumerable<ConsultorDTO>>.SuccessResponse(
                data: result,
                message: "Consultores obtenidos correctamente.",
                statusCode: StatusCodes.Status200OK);

            return Ok(response);
        }

        // GET /api/v1/consultores/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var consultor = await _consultoresService.GetByIdAsync(id);
            if (consultor == null)
            {
                var notFoundResponse = ApiResponse<ConsultorDTO>.ErrorResponse(
                    message: "Consultor no encontrado.",
                    statusCode: StatusCodes.Status404NotFound);

                return NotFound(notFoundResponse);
            }

            var response = ApiResponse<ConsultorDTO>.SuccessResponse(
                data: consultor,
                message: "Consultor obtenido correctamente.",
                statusCode: StatusCodes.Status200OK);

            return Ok(response);
        }

        // POST /api/v1/consultores (solo Admin)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] ConsultorCreateRequest request)
        {
            var dto = new ConsultorDTO
            {
                Nombre = request.Nombre,
                AreaEspecializacion = request.AreaEspecializacion,
                TarifaHora = request.TarifaHora,
                EmailCorporativo = request.EmailCorporativo,
                Activo = true // por defecto
            };

            var id = await _consultoresService.CreateAsync(dto);

            var response = ApiResponse<object>.SuccessResponse(
                data: new { id },
                message: "Consultor creado correctamente.",
                statusCode: StatusCodes.Status201Created);

            return StatusCode(StatusCodes.Status201Created, response);
        }

        // PUT /api/v1/consultores/{id} (solo Admin)
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] ConsultorUpdateRequest request)
        {
            var dto = new ConsultorDTO
            {
                ConsultorId = id,
                Nombre = request.Nombre,
                AreaEspecializacion = request.AreaEspecializacion,
                TarifaHora = request.TarifaHora,
                EmailCorporativo = request.EmailCorporativo,
                Activo = request.Activo
            };

            await _consultoresService.UpdateAsync(dto);

            var response = ApiResponse.SuccessResponse(
                message: "Consultor actualizado correctamente.",
                statusCode: StatusCodes.Status200OK);

            return Ok(response);
        }

        // DELETE /api/v1/consultores/{id} (solo Admin)
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _consultoresService.DeleteAsync(id);

            if (!deleted)
            {
                var notFoundResponse = ApiResponse.ErrorResponse(
                    message: "Consultor no encontrado.",
                    statusCode: StatusCodes.Status404NotFound);

                return NotFound(notFoundResponse);
            }

            var response = ApiResponse.SuccessResponse(
                message: "Consultor eliminado correctamente.",
                statusCode: StatusCodes.Status200OK);

            return Ok(response);
        }

    }
}
