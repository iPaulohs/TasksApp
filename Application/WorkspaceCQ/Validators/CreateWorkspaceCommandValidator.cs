using Application.WorkspaceCQ.Comands;
using FluentValidation;

namespace Application.WorkspaceCQ.Validators
{
    public class CreateWorkspaceCommandValidator : AbstractValidator<CreateWorkspaceCommand>
    {
        public CreateWorkspaceCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty()
                .WithMessage("O campo 'Title' não pode ser vazio.");

            RuleFor(x => x.UserId).NotEmpty()
                .WithMessage("O campo 'UserId' não pode ficar em branco."); ;
        }
    }
}
