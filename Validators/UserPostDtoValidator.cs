using Crud_API.Dtos.Post;
using Crud_API.Repositories.Interfaces;
using FluentValidation;
using FluentValidation.Results;

namespace Crud_API.Validators
{
    public class UserPostDtoValidator : AbstractValidator<UserPostDto>
    {
        private readonly IUserRepository _userRepository;

        public UserPostDtoValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name MUST be provided");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email MUST be provided")
                .EmailAddress().WithMessage("Invalid email, provide it with the correct format");

            RuleFor(x => x.Password)
                .MinimumLength(8).WithMessage("Password MUST be at least 8 characters long!")
                .Matches(@"[0-9]").WithMessage("Password MUST have at least one number ranging from 0 to 9")
                .Matches(@"[!@#$%]").WithMessage("Password MUST contain at least one special symbol, could be --->   !@#$%");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username MUST be provided");
        }

        public async Task<ValidationResult> ValidateAsyncWithUniqueness(UserPostDto userPostDto)
        {

            var validationResult = await ValidateAsync(userPostDto);
            if (!validationResult.IsValid)
            {
                return validationResult;
            }

            if (await _userRepository.EmailExistsAsync(userPostDto.Email))
            {
                validationResult.Errors.Add(new ValidationFailure(nameof(userPostDto.Email), "Email address already registered :(, try with another one!"));
            }

            if (await _userRepository.UserExistsAsync(userPostDto.UserName))
            {
                validationResult.Errors.Add(new ValidationFailure(nameof(userPostDto.UserName), "Username is already taken :( , try with another one"));
            }

            return validationResult;
        }
    }
}
