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
            var entity = new Consultor
            {
                Nombre = dto.Nombre,
                AreaEspecializacion = dto.AreaEspecializacion,
                TarifaHora = dto.TarifaHora,
                EmailCorporativo = dto.EmailCorporativo,
                Activo = true,
                FechaIngreso = DateTime.UtcNow
            };

            return await _repository.InsertAsync(entity);
        }

        public async Task UpdateAsync(ConsultorDTO dto)
        {
            if (!dto.ConsultorId.HasValue)
                throw new ArgumentException("ConsultorId es requerido para actualizar.");

            var entity = new Consultor
            {
                ConsultorId = dto.ConsultorId.Value,
                Nombre = dto.Nombre,
                AreaEspecializacion = dto.AreaEspecializacion,
                TarifaHora = dto.TarifaHora,
                EmailCorporativo = dto.EmailCorporativo,
                Activo = dto.Activo ?? true
            };

            await _repository.UpdateAsync(entity);
        }

        public Task DeleteAsync(int consultorId)
        {
            return _repository.DeleteAsync(consultorId);
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

