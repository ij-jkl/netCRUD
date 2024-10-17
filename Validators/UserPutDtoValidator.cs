using Crud_API.Dtos.Put;
using FluentValidation;

namespace Crud_API.Validators
{
    public class UserPutDtoValidator : AbstractValidator<UserPutDto>
    {
        public UserPutDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name MUST be provided");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email MUST be provided")
                .EmailAddress().WithMessage("Invalid email, provide it with the correct format");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password MUST be provided")
                .MinimumLength(8).WithMessage("Password HAST TO have least 8 letters long")
                .Matches("[A-Z]").WithMessage("Password HAST TO have least 1 CAPITAL letter")
                .Matches("[a-z]").WithMessage("Password HAST TO have least 1 LOWERCASE letter")
                .Matches("[0-9]").WithMessage("Password HAST TO have least 1 NUMBER");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username MUST be provided")
                .MinimumLength(4).WithMessage("Username HAST TO have at least 4 characters.");
        }
    }
}
