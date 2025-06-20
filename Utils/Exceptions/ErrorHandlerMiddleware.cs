using System.Net;
using System.Text.Json;
using Muuki.Exceptions;

namespace Muuki.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = ex switch
                {
                    UnauthorizedException or UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                    NotFoundException or KeyNotFoundException => (int)HttpStatusCode.NotFound,
                    BadRequestException or ArgumentException => (int)HttpStatusCode.BadRequest,
                    _ => (int)HttpStatusCode.InternalServerError
                };

                var result = JsonSerializer.Serialize(new
                {
                    success = false,
                    message = ex.Message,
                    data = (object)null
                });

                await context.Response.WriteAsync(result);
            }
        }
    }
}