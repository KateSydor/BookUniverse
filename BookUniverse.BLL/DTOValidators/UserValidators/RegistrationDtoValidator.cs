namespace BookUniverse.BLL.DTOValidators.UserValidators
{
    using BookUniverse.BLL.DTOs.UserDTOs;
    using BookUniverse.DAL.Constants.ValidationConstants;
    using FluentValidation;

    public class RegistrationDtoValidator : AbstractValidator<RegistrationDto>
    {
        public RegistrationDtoValidator()
        {
            RuleFor(user => user.Username)
                .NotEmpty()
                .MinimumLength(UserValidationConstants.USERNAME_MIN_LENGTH)
                .MaximumLength(UserValidationConstants.USERNAME_MAX_LENGTH);

            RuleFor(user => user.Email)
                .NotEmpty()
                .MinimumLength(UserValidationConstants.EMAIL_MIN_LENGTH)
                .MaximumLength(UserValidationConstants.EMAIL_MAX_LENGTH)
                .Matches(UserValidationConstants.EMAIL_PATTERN);

            RuleFor(user => user.Password)
                .NotEmpty()
                .MaximumLength(UserValidationConstants.PASSWORD_MAX_LENGTH)
                .Matches(UserValidationConstants.PASSWORD_PATTERN);

            RuleFor(user => user.RepeatPassword)
                .NotEmpty()
                .MaximumLength(UserValidationConstants.PASSWORD_MAX_LENGTH)
                .Matches(UserValidationConstants.PASSWORD_PATTERN);
        }
    }
}
