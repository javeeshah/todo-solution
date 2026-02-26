using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Todo.Api.Middleware
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

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception caught by global middleware");

                var env = context.RequestServices.GetRequiredService<IHostEnvironment>();

                context.Response.Clear();
                context.Response.ContentType = "application/problem+json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var traceId = context.TraceIdentifier;

                var problem = new ProblemDetails
                {
                    Type = "https://httpstatuses.io/500",
                    Title = "An unexpected error occurred.",
                    Status = StatusCodes.Status500InternalServerError,
                    Instance = context.Request.Path
                };

                // Development → include detailed exception info
                if (env.IsDevelopment())
                {
                    problem.Detail = ex.Message;
                    problem.Extensions["stackTrace"] = ex.StackTrace;
                }
                else
                {
                    // Production → safe generic message
                    problem.Detail = "An unexpected error occurred while processing the request.";
                }

                // Always include traceId for correlation
                problem.Extensions["traceId"] = traceId;

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var payload = JsonSerializer.Serialize(problem, options);
                await context.Response.WriteAsync(payload);
            }
        }
    }
}