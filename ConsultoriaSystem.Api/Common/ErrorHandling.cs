using System.Net;
using System.Text.Json;
using System.Data.SqlClient;

namespace ConsultoriaSystem.Api.Common
{
    public class ErrorHandling
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandling> _logger;

        public ErrorHandling(
            RequestDelegate next,
            ILogger<ErrorHandling> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL error on path {Path}", context.Request.Path);

                var statusCode = (int)HttpStatusCode.BadRequest;

                var response = ApiResponse.ErrorResponse(
                    message: ex.Message,                     
                    statusCode: statusCode,
                    errors: new[] { ex.Message });

                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json; charset=utf-8";

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled error on path {Path}", context.Request.Path);

                var statusCode = (int)HttpStatusCode.InternalServerError;

                var response = ApiResponse.ErrorResponse(
                    message: "Ha ocurrido un error inesperado. Intente nuevamente.",
                    statusCode: statusCode,
                    errors: null);

                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json; charset=utf-8";

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
        }
    }
}

