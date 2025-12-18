namespace ConsultoriaSystem.Api.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public IEnumerable<string>? Errors { get; set; }
        public int StatusCode { get; set; }

        public static ApiResponse<T> SuccessResponse(
            T data,
            string? message = null,
            int statusCode = StatusCodes.Status200OK)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                Errors = null,
                StatusCode = statusCode
            };
        }

        public static ApiResponse<T> ErrorResponse(
            string message,
            int statusCode = StatusCodes.Status400BadRequest,
            IEnumerable<string>? errors = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Data = default,
                Errors = errors,
                StatusCode = statusCode
            };
        }
    }

    public class ApiResponse : ApiResponse<object?>
    {
        public static ApiResponse SuccessResponse(
            string? message = null,
            int statusCode = StatusCodes.Status200OK)
        {
            return new ApiResponse
            {
                Success = true,
                Message = message,
                Data = null,
                Errors = null,
                StatusCode = statusCode
            };
        }

        public static ApiResponse ErrorResponse(
            string message,
            int statusCode = StatusCodes.Status400BadRequest,
            IEnumerable<string>? errors = null)
        {
            return new ApiResponse
            {
                Success = false,
                Message = message,
                Data = null,
                Errors = errors,
                StatusCode = statusCode
            };
        }
    }
}
