using System.Data;
using ConsultoriaSystem.Api.Entities;
using System.Data.SqlClient;

namespace ConsultoriaSystem.Api.Repositories
{
    public class PaquetesRepository : IPaquetesRepository
    {
        private readonly string _connectionString;

        public PaquetesRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("Connection string 'DefaultConnection' not found.");
        }

        public async Task<int> InsertAsync(PaqueteServicio paquete)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_Paquetes_Insert", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Nombre", paquete.Nombre);
            command.Parameters.AddWithValue("@DuracionHoras", paquete.DuracionHoras);
            command.Parameters.AddWithValue("@Costo", paquete.Costo);
            command.Parameters.AddWithValue("@Descripcion", (object?)paquete.Descripcion ?? DBNull.Value);

            await connection.OpenAsync();
            var result = await command.ExecuteScalarAsync(); 

            return Convert.ToInt32(result);
        }

        public async Task UpdateAsync(PaqueteServicio paquete)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_Paquetes_Update", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@PaqueteId", paquete.PaqueteId);
            command.Parameters.AddWithValue("@Nombre", paquete.Nombre);
            command.Parameters.AddWithValue("@DuracionHoras", paquete.DuracionHoras);
            command.Parameters.AddWithValue("@Costo", paquete.Costo);
            command.Parameters.AddWithValue("@Descripcion", (object?)paquete.Descripcion ?? DBNull.Value);
            command.Parameters.AddWithValue("@Activo", paquete.Activo);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task<int> DeleteAsync(int paqueteId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_Paquetes_Delete", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@PaqueteId", paqueteId);

            await connection.OpenAsync();
            int rows = await command.ExecuteNonQueryAsync();
            return rows;
        }

        public async Task<PaqueteServicio?> GetByIdAsync(int paqueteId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_Paquetes_GetById", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@PaqueteId", paqueteId);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                return null;

            return Map(reader);
        }

        public async Task<IEnumerable<PaqueteServicio>> GetAllAsync()
        {
            var list = new List<PaqueteServicio>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_Paquetes_GetAll", connection);
            command.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(Map(reader));
            }

            return list;
        }

        private static PaqueteServicio Map(SqlDataReader reader)
        {
            return new PaqueteServicio
            {
                PaqueteId = reader.GetInt32(reader.GetOrdinal("PaqueteId")),
                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                DuracionHoras = reader.GetInt32(reader.GetOrdinal("DuracionHoras")),
                Costo = reader.GetDecimal(reader.GetOrdinal("Costo")),
                Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("Descripcion")),
                Activo = reader.GetBoolean(reader.GetOrdinal("Activo")),
                FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FechaCreacion"))
            };
        }
    }
}
