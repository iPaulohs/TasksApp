using Application.UserCQ.Commands;
using FluentValidation;

namespace Application.UserCQ.Validators
{
    public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenCommandValidator()
        {
            RuleFor(x => x.RefreshToken).NotEmpty().WithMessage("Campo 'Token' nao pode estar vazio.");
            RuleFor(x => x.ExpiredToken).NotEmpty().WithMessage("Campo 'ExpiredToken' nao pode estar vazio.");
        }
    }
}
