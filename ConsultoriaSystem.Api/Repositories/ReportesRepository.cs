using System.Data;
using ConsultoriaSystem.Api.Dtos;
using System.Data.SqlClient;

namespace ConsultoriaSystem.Api.Repositories
{
    public class ReportesRepository : IReportesRepository
    {
        private readonly string _connectionString;

        public ReportesRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("Connection string 'DefaultConnection' not found.");
        }

        // 🔹 Reporte: Paquetes por área
        public async Task<IEnumerable<ReportePaquetesPorAreaDTO>> GetPaquetesPorAreaAsync()
        {
            var list = new List<ReportePaquetesPorAreaDTO>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_Reportes_PaquetesPorArea", connection);
            command.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var item = new ReportePaquetesPorAreaDTO
                {
                    Area = reader.GetString(reader.GetOrdinal("Area")),
                    TotalConsultores = reader.GetInt32(reader.GetOrdinal("TotalConsultores")),
                    TotalPaquetes = reader.GetInt32(reader.GetOrdinal("TotalPaquetes")),
                    TotalFacturacionEstimada = reader.GetDecimal(reader.GetOrdinal("TotalFacturacionEstimada"))
                };

                list.Add(item);
            }

            return list;
        }

        // 🔹 Reporte: Consultores top facturación
        public async Task<IEnumerable<ReporteConsultorTopFacturacionDto>> GetConsultoresTopFacturacionAsync()
        {
            var list = new List<ReporteConsultorTopFacturacionDto>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_Reportes_ConsultoresTopFacturacion", connection);
            command.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var item = new ReporteConsultorTopFacturacionDto
                {
                    ConsultorId = reader.GetInt32(reader.GetOrdinal("ConsultorId")),
                    Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                    AreaEspecializacion = reader.GetString(reader.GetOrdinal("AreaEspecializacion")),
                    EmailCorporativo = reader.GetString(reader.GetOrdinal("EmailCorporativo")),
                    TotalPaquetesAsignados = reader.GetInt32(reader.GetOrdinal("TotalPaquetesAsignados")),
                    TotalHoras = reader.GetInt32(reader.GetOrdinal("TotalHoras")),
                    TotalFacturacionEstimada = reader.GetDecimal(reader.GetOrdinal("TotalFacturacionEstimada"))
                };

                list.Add(item);
            }

            return list;
        }
    }
}
