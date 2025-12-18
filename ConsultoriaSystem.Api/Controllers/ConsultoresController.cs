using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ConsultoriaSystem.Api.Services;
using ConsultoriaSystem.Api.Dtos;

namespace ConsultoriaSystem.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize] // todos requieren token 
    public class ConsultoresController : ControllerBase
    {
        private readonly IConsultoresService _consultoresService;
        private readonly IConsultorPaqueteService _consultorPaqueteService;

        public ConsultoresController(
            IConsultoresService consultoresService,
            IConsultorPaqueteService consultorPaqueteService)
        {
            _consultoresService = consultoresService;
            _consultorPaqueteService = consultorPaqueteService;
        }

        // GET /api/v1/consultores
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _consultoresService.GetAllAsync(); // devuelve List<ConsultorDto>
            return Ok(result);
        }

        // GET /api/v1/consultores/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var consultor = await _consultoresService.GetByIdAsync(id);
            if (consultor == null)
                return NotFound();

            return Ok(consultor);
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
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
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
            return NoContent();
        }

        // DELETE /api/v1/consultores/{id} (solo Admin)
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _consultoresService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("{id:int}/paquetes/{paqueteId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AsignarPaquete(int id, int paqueteId)
        {
            var asignacionId = await _consultorPaqueteService.AsignarPaqueteAsync(id, paqueteId);
            return Ok(new { asignacionId });
        }

        // DELETE /api/v1/consultores/{id}/paquetes/{paqueteId}
        // Quitar paquete de consultor (solo Admin)
        [HttpDelete("{id:int}/paquetes/{paqueteId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DesasignarPaquete(int id, int paqueteId)
        {
            await _consultorPaqueteService.DesasignarPaqueteAsync(id, paqueteId);
            return NoContent();
        }
    }
}

