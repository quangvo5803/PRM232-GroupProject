using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Utilities.Exceptions
{
    public class ValidationExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidationExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (CustomValidationException ex)
            {
                await HandleValidationExceptionAsync(httpContext, ex);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static async Task HandleValidationExceptionAsync(
            HttpContext context,
            CustomValidationException exception
        )
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            var response = new { message = exception.Message, errors = exception.Errors };

            var json = JsonConvert.SerializeObject(response); // thay thế JsonSerializer.Serialize
            await context.Response.WriteAsync(json);
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var response = new { message = "An unexpected error occurred." };

            var json = JsonConvert.SerializeObject(response); // thay thế JsonSerializer.Serialize
            await context.Response.WriteAsync(json);
        }
    }
}
