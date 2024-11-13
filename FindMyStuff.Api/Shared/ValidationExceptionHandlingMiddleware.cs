using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace FindMyStuff.Api.Shared
{
    public class ValidationExceptionHandlingMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException exception)
            {

                var validationErrors = string.Join("\n", exception.Errors.Select(x => x.Value));

                var dict = new Dictionary<string, string>();

                Log.Information("Validation failed on: {@instance}, Errors: {@errors}", 
                        exception?.Context?.Name, exception?.Errors.Select(x => x.Value));

                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Type = "ValidationFailure",
                    Title = "Validation error",
                    Detail = "One or more validation errors has occurred"
                };

                if (exception?.Errors is not null)
                {
                    problemDetails.Extensions["errors"] = exception.Errors;
                }

                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                await context.Response.WriteAsJsonAsync(problemDetails);
            }
        }
    }

    public static class ValidationExtensions
    {
        public static IApplicationBuilder UseValidationExceptionHandlingMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ValidationExceptionHandlingMiddleware>();
        }
    }
}
