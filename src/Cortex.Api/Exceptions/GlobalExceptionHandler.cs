using Azure.Core.GeoJson;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Cortex.Api.Exceptions
{
    public class GlobalExceptionHandler :IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Unhandled exception occured: {Message}", exception.Message);

            var (statusCode, title) = exception switch
            {
                UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "UnAuthorized"),
                ArgumentNullException => (HttpStatusCode.BadRequest, "Bad Request"),
                ArgumentException => (HttpStatusCode.BadRequest, "Bad Request"),
                KeyNotFoundException => (HttpStatusCode.NotFound, "Not Found"),
                InvalidOperationException => (HttpStatusCode.BadRequest, "Invalid Operations"),
                   _ => (HttpStatusCode.InternalServerError, "Internal Server Error")
            };
            var problemDetails = new ProblemDetails
            {
                Status = (int)statusCode,
                Title = title,
                Detail = exception.Message,
                Instance = httpContext.Request.Path
            };
            httpContext.Response.StatusCode = (int)statusCode;
            httpContext.Response.ContentType = "application/json";

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;

        }

    }
}
