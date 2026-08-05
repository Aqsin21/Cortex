using Cortex.Module.Issues.Application.Issues.CreateIssue.Cortex.Module.Issues.Application.Issues.CreateIssue;
using FluentValidation;
namespace Cortex.Module.Issues.Application.Issues.CreateIssue
{
    public class CreateIssueCommandValidator : AbstractValidator<CreateIssueCommand>
    {
        public CreateIssueCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(250).WithMessage("Title cannot exceed 250 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.");

            RuleFor(x => x.ProjectId)
                .NotEmpty().WithMessage("Project ID is required.");

            RuleFor(x => x.Priority)
                .IsInEnum().WithMessage("Invalid priority value.");
        }
    }
}
