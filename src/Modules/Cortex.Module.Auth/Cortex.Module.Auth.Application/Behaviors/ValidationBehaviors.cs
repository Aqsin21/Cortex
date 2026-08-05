using FluentValidation;
using MediatR;
using System.Security.AccessControl;

namespace Cortex.Module.Auth.Application.Behaviors
{
    public class ValidationBehaviors<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest :notnull
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        
        public ValidationBehaviors(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators= validators;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (!_validators.Any())
                return await next();

            var context = new ValidationContext<TRequest>(request);
            var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Any())
                throw new ValidationException(failures);

            return await next();
        }

        }
    }
