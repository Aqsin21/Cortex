using FluentValidation;
namespace Cortex.Module.Issues.Application.WorkSpaces.CreateWorkSpace
{
    public class CreateWorkspaceCommandValidator : AbstractValidator<CreateWorkspaceCommand>
    {
        public CreateWorkspaceCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Workspace name is required.")
                .MaximumLength(100).WithMessage("Workspace name cannot exceed 100 characters.");

            RuleFor(x => x.OwnerId)
                .NotEmpty().WithMessage("Owner ID is required.");

            RuleFor(x => x.OwnerFullName)
                .NotEmpty().WithMessage("Owner full name is required.")
                .MaximumLength(200).WithMessage("Full name cannot exceed 200 characters.");
        }
    }

}
