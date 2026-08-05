using FluentValidation;
namespace Cortex.Module.Issues.Application.WorkSpaces.AddMember
{
    public class AddWorkspaceMemberCommandValidator : AbstractValidator<AddWorkspaceMemberCommand>
    {
        public AddWorkspaceMemberCommandValidator()
        {
            RuleFor(x => x.WorkspaceId)
                .NotEmpty().WithMessage("Workspace ID is required.");

            RuleFor(x => x.TargetUserId)
                .NotEmpty().WithMessage("Target user ID is required.");

            RuleFor(x => x.TargetFullName)
                .NotEmpty().WithMessage("Target full name is required.");

            RuleFor(x => x.RoleName)
                .NotEmpty().WithMessage("Role name is required.");
        }
    }
}
