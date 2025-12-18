using ConsultoriaSystem.Api.Entities;

namespace ConsultoriaSystem.Api.Repositories
{
    public interface IUsuariosRepository
    {
        Task<Usuarios?> LoginAsync(string email, string password);
    }
}
