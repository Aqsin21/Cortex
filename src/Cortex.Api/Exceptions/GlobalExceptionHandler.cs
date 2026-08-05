using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Cortex.Api.Exceptions
{
    public class GlobalExceptionHandler : IExceptionHandler
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
            _logger.LogError(exception, "Unhandled exception occurred: {Message}", exception.Message);

            var (statusCode, title) = exception switch
            {
                ValidationException => (HttpStatusCode.BadRequest, "Validation Error"),
                UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "Unauthorized"),
                ArgumentNullException => (HttpStatusCode.BadRequest, "Bad Request"),
                ArgumentException => (HttpStatusCode.BadRequest, "Bad Request"),
                KeyNotFoundException => (HttpStatusCode.NotFound, "Not Found"),
                InvalidOperationException => (HttpStatusCode.BadRequest, "Invalid Operation"),
                _ => (HttpStatusCode.InternalServerError, "Internal Server Error")
            };

            var problemDetails = new ProblemDetails
            {
                Status = (int)statusCode,
                Title = title,
                Detail = exception switch
                {
                    ValidationException => "One or more validation failures have occurred.",
                    _ => exception.Message
                },
                Instance = httpContext.Request.Path
            };

            
            if (exception is ValidationException validationException)
            {
                var errors = validationException.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        failureGroup => failureGroup.Key,
                        failureGroup => failureGroup.Select(e => e.ErrorMessage).ToArray()
                    );

                problemDetails.Extensions.Add("errors", errors);
            }

            httpContext.Response.StatusCode = (int)statusCode;
            httpContext.Response.ContentType = "application/json";

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}