using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ConsultoriaSystem.Api.Services;
using ConsultoriaSystem.Api.Dtos;

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
            return Ok(result); 
        }

        // GET /api/v1/paquetes/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var paquete = await _paquetesService.GetByIdAsync(id);
            if (paquete == null) return NotFound();

            return Ok(paquete);
        }

        // POST /api/v1/paquetes  (solo Admin)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] PaqueteCreateRequest request)
        {
            // Mapeas al DTO que ya usas en el servicio
            var dto = new PaqueteDTO
            {
                Nombre = request.Nombre,
                DuracionHoras = request.DuracionHoras,
                Costo = request.Costo,
                Descripcion = request.Descripcion
                // Id y FechaCreacion los pone la bd
            };

            var id = await _paquetesService.CreateAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        // PUT /api/v1/paquetes/{id}  (solo Admin)
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] PaqueteUpdateRequest request)
        {
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
            return NoContent();
        }

        // DELETE /api/v1/paquetes/{id}  (solo Admin)
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _paquetesService.DeleteAsync(id);
            return NoContent();
        }
    }
}
