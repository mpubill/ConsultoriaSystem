using System.Data;
using ConsultoriaSystem.Api.Entities;
using System.Data.SqlClient;

namespace ConsultoriaSystem.Api.Repositories

{
    public class UsuariosRepository : IUsuariosRepository
    {
        private readonly string _connectionString;

        public UsuariosRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("Connection string 'DefaultConnection' not found.");
        }

        public async Task<Usuarios?> LoginAsync(string email, string password)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_Usuarios_Login", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@Password", password);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                return null;

            return new Usuarios
            {
                UsuarioId = reader.GetInt32(reader.GetOrdinal("UsuarioId")),
                Email = reader.GetString(reader.GetOrdinal("Email")),
                Rol = reader.GetString(reader.GetOrdinal("Rol"))
            };
        }
    }
}
