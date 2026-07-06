using Cortex.Module.Auth.Application.Abstraction;
using MediatR;
namespace Cortex.Module.Auth.Application.Login
{
    public class LoginCommandHandler: IRequestHandler<LoginCommand, LoginResult>
    {
        private readonly IIdentityService _identityService;
        private readonly ITokenService _tokenService;
        public LoginCommandHandler(IIdentityService identityService, ITokenService tokenService)
        {
            _identityService = identityService;
            _tokenService = tokenService;
        }
        public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.ValidateCredentialsAsync(request.Email, request.Password);

            if (!result.Succeeded)
            {
                return new LoginResult { Succeeded = false, Error = result.Error };
            }

            var token = _tokenService.GenerateToken(result.UserId!, result.Email!);

            return new LoginResult { Succeeded = true, Token = token };
        }


    }
}
