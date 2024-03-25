using Application.WorkspaceCQ.Comands;
using FluentValidation;

namespace Application.WorkspaceCQ.Validators
{
    public class UpdateWorkspaceCommandValidator : AbstractValidator<UpdateWorkspaceCommand>
    {
        public UpdateWorkspaceCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty()
               .WithMessage("O campo 'Title' não pode ser vazio.");
            RuleFor(x => x.Id).NotEmpty()
               .WithMessage("O campo 'Id' não pode ser vazio.");
        }
    }
}
