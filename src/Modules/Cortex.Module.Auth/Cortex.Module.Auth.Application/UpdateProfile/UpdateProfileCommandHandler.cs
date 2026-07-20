using Cortex.Module.Auth.Application.Abstraction;
using MediatR;

namespace Cortex.Module.Auth.Application.UpdateProfile
{
    public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, UpdateProfileResult>
    {
        private readonly IIdentityService _identityService;

        public UpdateProfileCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<UpdateProfileResult> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.UpdateProfileAsync(
                request.UserId, request.FirstName, request.LastName);

            return new UpdateProfileResult
            {
                Succeeded = result.Succeeded,
                Error = result.Errors.FirstOrDefault()
            };
        }
    }
}