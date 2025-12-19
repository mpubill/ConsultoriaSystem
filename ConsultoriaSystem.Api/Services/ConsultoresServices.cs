using ConsultoriaSystem.Api.Dtos;
using ConsultoriaSystem.Api.Entities;
using ConsultoriaSystem.Api.Repositories;

namespace ConsultoriaSystem.Api.Services
{
    public class ConsultoresService : IConsultoresService
    {
        private readonly IConsultoresRepository _repository;

        public ConsultoresService(IConsultoresRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> CreateAsync(ConsultorDTO dto)
        {
            if (await _repository.EmailExistsAsync(dto.EmailCorporativo))
                throw new InvalidOperationException("El email corporativo ya existe.");

            if (await _repository.NombreAreaExistsAsync(dto.Nombre, dto.AreaEspecializacion))
                throw new InvalidOperationException("Ya existe un consultor con ese nombre en esa área de especialización.");

            var entity = new Consultor
            {
                Nombre = dto.Nombre,
                AreaEspecializacion = dto.AreaEspecializacion,
                TarifaHora = dto.TarifaHora,
                EmailCorporativo = dto.EmailCorporativo,
                Activo = dto.Activo ?? true,      // por defecto activo
                FechaIngreso = DateTime.UtcNow
            };

            return await _repository.InsertAsync(entity);
        }

        // UPDATE
        public async Task UpdateAsync(ConsultorDTO dto)
        {
            if (!dto.ConsultorId.HasValue)
                throw new ArgumentException("ConsultorId es requerido para actualizar.");

            var existing = await _repository.GetByIdAsync(dto.ConsultorId.Value);
            if (existing == null)
                throw new InvalidOperationException("El consultor no existe.");

            if (!string.Equals(existing.EmailCorporativo, dto.EmailCorporativo, StringComparison.OrdinalIgnoreCase)
                && await _repository.EmailExistsAsync(dto.EmailCorporativo))
            {
                throw new InvalidOperationException("El email corporativo ya existe.");
            }

            bool nombreCambio = !string.Equals(existing.Nombre, dto.Nombre, StringComparison.OrdinalIgnoreCase);
            bool areaCambio = !string.Equals(existing.AreaEspecializacion, dto.AreaEspecializacion, StringComparison.OrdinalIgnoreCase);

            if ((nombreCambio || areaCambio)
                && await _repository.NombreAreaExistsAsync(dto.Nombre, dto.AreaEspecializacion))
            {
                throw new InvalidOperationException("Ya existe un consultor con ese nombre en esa área de especialización.");
            }

            existing.Nombre = dto.Nombre;
            existing.AreaEspecializacion = dto.AreaEspecializacion;
            existing.TarifaHora = dto.TarifaHora;
            existing.EmailCorporativo = dto.EmailCorporativo;
            existing.Activo = dto.Activo ?? existing.Activo;

            await _repository.UpdateAsync(existing);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var rows = await _repository.DeleteAsync(id);
            return rows > 0;
        }

        public async Task<ConsultorDTO?> GetByIdAsync(int consultorId)
        {
            var entity = await _repository.GetByIdAsync(consultorId);
            if (entity == null)
                return null;

            return MapToDto(entity);
        }

        public async Task<IEnumerable<ConsultorDTO>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return entities.Select(MapToDto).ToList();
        }

        private static ConsultorDTO MapToDto(Consultor c) =>
            new ConsultorDTO
            {
                ConsultorId = c.ConsultorId,
                Nombre = c.Nombre,
                AreaEspecializacion = c.AreaEspecializacion,
                TarifaHora = c.TarifaHora,
                EmailCorporativo = c.EmailCorporativo,
                Activo = c.Activo,
                FechaIngreso = c.FechaIngreso
            };
    }
}
