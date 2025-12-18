using ConsultoriaSystem.Api.Common;
using ConsultoriaSystem.Api.Dtos;
using ConsultoriaSystem.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConsultoriaSystem.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UsuariosDTO request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                var apiResponse = ApiResponse<LogginResponseDTO>.ErrorResponse(
                    message: "Errores de validación",
                    statusCode: StatusCodes.Status400BadRequest,
                    errors: errors
                );

                return StatusCode(apiResponse.StatusCode, apiResponse);
            }

            var response = await _authService.LoginAsync(request);

            if (response == null)
            {
                var apiResponse = ApiResponse<LogginResponseDTO>.ErrorResponse(
                    message: "Credenciales inválidas",
                    statusCode: StatusCodes.Status401Unauthorized,
                    errors: new[] { "Usuario o contraseña incorrectos." }
                );

                return StatusCode(apiResponse.StatusCode, apiResponse);
            }

            var successResponse = ApiResponse<LogginResponseDTO>.SuccessResponse(
                data: response,
                message: "Login exitoso"
            );

            return Ok(successResponse);
        }
    }

}
