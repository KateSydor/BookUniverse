namespace BookUniverse.BLL.DTOValidators.UserValidators
{
    using BookUniverse.BLL.DTOs.UserDTOs;
    using BookUniverse.DAL.Constants.ValidationConstants;
    using FluentValidation;

    public class EditUserDtoValidator : AbstractValidator<EditUserDto>
    {
        public EditUserDtoValidator()
        {
            RuleFor(user => user.Username)
                .MinimumLength(UserValidationConstants.USERNAME_MIN_LENGTH)
                .MaximumLength(UserValidationConstants.USERNAME_MAX_LENGTH);

            RuleFor(user => user.Email)
                .MinimumLength(UserValidationConstants.EMAIL_MIN_LENGTH)
                .MaximumLength(UserValidationConstants.EMAIL_MAX_LENGTH)
                .Matches(UserValidationConstants.EMAIL_PATTERN);
        }
    }
}
