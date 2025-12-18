using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ConsultoriaSystem.Api.Repositories
{
    public class ConsultorPaqueteRepository : IConsultorPaqueteRepository
    {
        private readonly string _connectionString;

        public ConsultorPaqueteRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<int> AsignarPaqueteAsync(int consultorId, int paqueteId)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_ConsultorPaquete_Asignar", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ConsultorId", consultorId);
            cmd.Parameters.AddWithValue("@PaqueteId", paqueteId);

            await conn.OpenAsync();

            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task DesasignarPaqueteAsync(int consultorId, int paqueteId)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_ConsultorPaquete_Desasignar", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ConsultorId", consultorId);
            cmd.Parameters.AddWithValue("@PaqueteId", paqueteId);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
