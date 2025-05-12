using CurriculumAdapter.API.Response;
using System.Text.Json;

namespace CurriculumAdapter.API.Middleware
{
    public class ExceptionHandler
    {
        private readonly RequestDelegate _next;

        public ExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
                Console.WriteLine(ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            int httpStatus = 400;
            string message = exception.Message;

            context.Response.ContentType = "application/json";

            context.Response.StatusCode = httpStatus;

            var error = new APIResponse<string>(false, 400, message);

            await context.Response.WriteAsync(JsonSerializer.Serialize(error));
        }
    }
}
