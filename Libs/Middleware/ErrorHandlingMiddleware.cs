using CondigiBack.Libs.Responses;
using System.Net;
using System.Text.Json;

namespace CondigiBack.Libs.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
                if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
                {
                    context.Response.Clear();
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    var errorResponse = new ErrorResponse<object>("Debe tener un token de sesión", "No autorizado", 401);
                    var jsonResponse = JsonSerializer.Serialize(errorResponse);
                    await context.Response.WriteAsync(jsonResponse);
                }else if(context.Response.StatusCode == (int)HttpStatusCode.Forbidden)
                {
                    context.Response.Clear();
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    var errorResponse = new ErrorResponse<object>("No tiene permisos para realizar esta acción", "Prohibido", 403);
                    var jsonResponse = JsonSerializer.Serialize(errorResponse);
                    await context.Response.WriteAsync(jsonResponse);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                context.Response.ContentType = "application/json";
                var errorResponse = new ErrorResponse<object>("Ha ocurrido un error en el servidor", "Error", 500);
                var jsonResponse = JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(jsonResponse);
            }

        }
    }
}
