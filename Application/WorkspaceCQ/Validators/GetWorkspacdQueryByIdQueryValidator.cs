using Application.WorkspaceCQ.Queries;
using FluentValidation;

namespace Application.WorkspaceCQ.Validators
{
    public class GetWorkspacdQueryByIdQueryValidator : AbstractValidator<GetWorkspaceByIdQuery>
    {
        public GetWorkspacdQueryByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("O campo 'Id' não pode estar vazio.");
                
        }
    }
}
