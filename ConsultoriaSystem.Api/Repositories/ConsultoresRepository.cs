using System.Data.SqlClient;
using System.Data;
using ConsultoriaSystem.Api.Entities;

namespace ConsultoriaSystem.Api.Repositories
{
    public class ConsultoresRepository : IConsultoresRepository
    {
        private readonly string _connectionString;

        public ConsultoresRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("Connection string 'DefaultConnection' not found.");
        }

        public async Task<int> InsertAsync(Consultor consultor)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_Consultores_Insert", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Nombre", consultor.Nombre);
            command.Parameters.AddWithValue("@AreaEspecializacion", consultor.AreaEspecializacion);
            command.Parameters.AddWithValue("@TarifaHora", consultor.TarifaHora);
            command.Parameters.AddWithValue("@EmailCorporativo", consultor.EmailCorporativo);

            await connection.OpenAsync();
            var result = await command.ExecuteScalarAsync();

            return Convert.ToInt32(result);
        }

        public async Task UpdateAsync(Consultor consultor)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_Consultores_Update", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@ConsultorId", consultor.ConsultorId);
            command.Parameters.AddWithValue("@Nombre", consultor.Nombre);
            command.Parameters.AddWithValue("@AreaEspecializacion", consultor.AreaEspecializacion);
            command.Parameters.AddWithValue("@TarifaHora", consultor.TarifaHora);
            command.Parameters.AddWithValue("@EmailCorporativo", consultor.EmailCorporativo);
            command.Parameters.AddWithValue("@Activo", consultor.Activo);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task<int> DeleteAsync(int consultorId)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_Consultores_Delete", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ConsultorId", consultorId);

            await conn.OpenAsync();
            int rows = await cmd.ExecuteNonQueryAsync();
            return rows;
        }


        public async Task<Consultor?> GetByIdAsync(int consultorId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_Consultores_GetById", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@ConsultorId", consultorId);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                return null;

            return Map(reader);
        }

        public async Task<IEnumerable<Consultor>> GetAllAsync()
        {
            var list = new List<Consultor>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_Consultores_GetAll", connection);
            command.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(Map(reader));
            }

            return list;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_Consultores_EmailExists", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@EmailCorporativo", email);

            await connection.OpenAsync();
            var result = await command.ExecuteScalarAsync();

            // asumimos que el SP devuelve COUNT(*)
            int count = Convert.ToInt32(result);
            return count > 0;
        }

        public async Task<bool> NombreAreaExistsAsync(string nombre, string area)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_Consultores_NombreAreaExists", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Nombre", nombre);
            command.Parameters.AddWithValue("@AreaEspecializacion", area);

            await connection.OpenAsync();
            var result = await command.ExecuteScalarAsync();

            int count = Convert.ToInt32(result);
            return count > 0;
        }

        private static Consultor Map(SqlDataReader reader)
        {
            return new Consultor
            {
                ConsultorId = reader.GetInt32(reader.GetOrdinal("ConsultorId")),
                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                AreaEspecializacion = reader.GetString(reader.GetOrdinal("AreaEspecializacion")),
                TarifaHora = reader.GetDecimal(reader.GetOrdinal("TarifaHora")),
                EmailCorporativo = reader.GetString(reader.GetOrdinal("EmailCorporativo")),
                Activo = reader.GetBoolean(reader.GetOrdinal("Activo")),
                FechaIngreso = reader.GetDateTime(reader.GetOrdinal("FechaIngreso"))
            };
        }
    }
}