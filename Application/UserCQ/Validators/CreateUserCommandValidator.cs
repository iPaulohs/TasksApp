using Application.UserCQ.Commands;
using FluentValidation;
using Infra.Persistence;

namespace Application.UserCQ.Validators
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        private readonly TasksDbContext _context;
        public CreateUserCommandValidator(TasksDbContext context)
        {
            _context = context;

            RuleFor(x => x.Name).NotEmpty().WithMessage("O campo 'Nome' não pode estar vazio.");
            RuleFor(x => x.Email).NotEmpty().WithMessage("O campo 'Email' não pode estar vazio.").EmailAddress();
            RuleFor(x => x.Password).NotEmpty().WithMessage("O campo 'Senha' não pode estar vazio.").MinimumLength(6);
            RuleFor(x => x.Username).NotEmpty().WithMessage("O campo 'Username' não pode estar vazio.");
        }



    }
}
