using Crud_API.Dtos.Post;
using FluentValidation;

namespace Crud_API.Validators
{
    public class UserPostDtoValidator : AbstractValidator<UserPostDto>
    {
        public UserPostDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name MUST be provided");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email MUST be provided").EmailAddress().WithMessage("Invalid format");
            RuleFor(x => x.Password)
            .MinimumLength(8)
            .WithMessage("Password must be at least 8 characters long!")
            .Matches(@"[0-9]").WithMessage("Password MUST have at least one number ranging from 0 to 9")
            .Matches(@"[!@#$%]").WithMessage("Password must contain at least one special symbol, could be --->   !@#$%");
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Username MUST be provided");
        }
    }
}
