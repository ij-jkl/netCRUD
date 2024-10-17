using Crud_API.Dtos.Login;
using FluentValidation;

namespace Crud_API.Validators
{
    public class LoginDtoValidator : AbstractValidator<UserLoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Username MUST be provided");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password MUST be provided");
        }
    }
}
