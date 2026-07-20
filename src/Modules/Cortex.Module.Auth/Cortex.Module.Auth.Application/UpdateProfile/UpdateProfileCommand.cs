using MediatR;

namespace Cortex.Module.Auth.Application.UpdateProfile
{
    public class UpdateProfileCommand : IRequest<UpdateProfileResult>
    {
        public required string UserId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
    }

    public class UpdateProfileResult
    {
        public bool Succeeded { get; set; }
        public string? Error { get; set; }
    }
}