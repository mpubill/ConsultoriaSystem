using ConsultoriaSystem.Api.Dtos;
using ConsultoriaSystem.Api.Entities;
using ConsultoriaSystem.Api.Repositories;

namespace ConsultoriaSystem.Api.Services
{
    public class PaquetesService : IPaquetesService
    {
        private readonly IPaquetesRepository _repository;

        public PaquetesService(IPaquetesRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> CreateAsync(PaqueteDTO dto)
        {
            var entity = new PaqueteServicio
            {
                Nombre = dto.Nombre,
                DuracionHoras = dto.DuracionHoras,
                Costo = dto.Costo,
                Descripcion = dto.Descripcion,
                Activo = true
            };

            return await _repository.InsertAsync(entity);
        }

        public async Task UpdateAsync(PaqueteDTO dto)
        {
            if (!dto.PaqueteId.HasValue)
                throw new ArgumentException("PaqueteId es requerido para actualizar.");

            var entity = new PaqueteServicio
            {
                PaqueteId = dto.PaqueteId.Value,
                Nombre = dto.Nombre,
                DuracionHoras = dto.DuracionHoras,
                Costo = dto.Costo,
                Descripcion = dto.Descripcion,
                Activo = dto.Activo ?? true
            };

            await _repository.UpdateAsync(entity);
        }

        public Task DeleteAsync(int paqueteId)
        {
            return _repository.DeleteAsync(paqueteId);
        }

        public async Task<PaqueteDTO?> GetByIdAsync(int paqueteId)
        {
            var entity = await _repository.GetByIdAsync(paqueteId);
            if (entity == null)
                return null;

            return MapToDto(entity);
        }

        public async Task<IEnumerable<PaqueteDTO>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return entities.Select(MapToDto).ToList();
        }

        private static PaqueteDTO MapToDto(PaqueteServicio p) =>
            new PaqueteDTO
            {
                PaqueteId = p.PaqueteId,
                Nombre = p.Nombre,
                DuracionHoras = p.DuracionHoras,
                Costo = p.Costo,
                Descripcion = p.Descripcion,
                Activo = p.Activo,
                FechaCreacion = p.FechaCreacion
            };
    }
}
