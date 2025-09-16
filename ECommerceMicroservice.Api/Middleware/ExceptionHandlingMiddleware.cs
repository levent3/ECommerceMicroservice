using System.Net;
using System.Text.Json;

namespace ECommerceMicroservice.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                // İstek işlem hattındaki bir sonraki middleware'i çağırır
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                // Beklenmeyen hatayı loglar
                _logger.LogError(ex, "Beklenmeyen bir hata oluştu: {Message}", ex.Message);

                // Hata yanıtının formatını ayarlar
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                // Kullanıcıya gönderilecek JSON yanıtını hazırlar
                var response = new
                {
                    status = (int)HttpStatusCode.InternalServerError,
                    message = "Beklenmeyen bir hata oluştu. Lütfen daha sonra tekrar deneyin."
                };

                var jsonResponse = JsonSerializer.Serialize(response);
                await httpContext.Response.WriteAsync(jsonResponse);
            }
        }
    }
}
