using Application.UserCQ.Commands;
using FluentValidation;

namespace Application.UserCQ.Validators
{
    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Digite o email para fazer o login.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Digite o email para fazer o login.");
        }
    }
}
