using Cortex.Module.Auth.Application.Abstraction;
using MediatR;
namespace Cortex.Module.Auth.Application.Register
{
    public class RegisterCommandHandler: IRequestHandler<RegisterCommand , RegisterResult>
    {
        private readonly IIdentityService _identityService;
        public RegisterCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        public async Task<RegisterResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.RegisterAsync(
                 request.Email, request.Password, request.FirstName, request.LastName);
            return new RegisterResult
            {
                Succeeded = result.Succeeded,
                UserId = result.UserId,
                Errors = result.Errors
            };
        }
    }
}
