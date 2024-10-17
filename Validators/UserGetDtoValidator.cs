using Crud_API.Dtos.Get;
using FluentValidation;

namespace Crud_API.Validators
{
    public class UserGetDtoValidator : AbstractValidator<UserGetDto>
    {
        public UserGetDtoValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("ID must be bigger than cero (0)");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name MUST be provided");
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Username MUST be provided");
        }
    }
}
