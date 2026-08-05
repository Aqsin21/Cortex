using FluentValidation;
namespace Cortex.Module.Issues.Application.Projects.CreateProject
{
    public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
    {
        public CreateProjectCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Project name is required.")
                .MaximumLength(150).WithMessage("Project name cannot exceed 150 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");

            RuleFor(x => x.WorkspaceId)
                .NotEmpty().WithMessage("Workspace ID is required.");

            RuleFor(x => x.EndDate)
                .GreaterThan(DateTime.UtcNow).WithMessage("End date must be in the future.");
        }
    }
}
