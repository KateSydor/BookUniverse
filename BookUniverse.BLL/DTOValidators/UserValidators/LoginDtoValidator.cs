namespace BookUniverse.BLL.DTOValidators.UserValidators
{
    using BookUniverse.BLL.DTOs.UserDTOs;
    using BookUniverse.DAL.Constants.ValidationConstants;
    using FluentValidation;

    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(user => user.Username)
                .NotEmpty()
                .MinimumLength(UserValidationConstants.USERNAME_MIN_LENGTH)
                .MaximumLength(UserValidationConstants.USERNAME_MAX_LENGTH);

            RuleFor(user => user.Password)
                .NotEmpty()
                .MaximumLength(UserValidationConstants.PASSWORD_MAX_LENGTH)
                .Matches(UserValidationConstants.PASSWORD_PATTERN);
        }
    }
}
