namespace BookUniverse.BLL.DTOs
{
    using System.ComponentModel.DataAnnotations;
    using BookUniverse.DAL.Constants.ValidationConstants;

    public class RegistrationDto
    {
        public string? Username { get; set; }

        [RegularExpression(UserValidationConstants.EMAIL_PATTERN, ErrorMessage = UserValidationConstants.NOT_VALID_EMAIL)]
        public string? Email { get; set; }

        [RegularExpression(UserValidationConstants.PASSWORD_PATTERN, ErrorMessage = UserValidationConstants.NOT_VALID_PASSWORD)]
        public string? Password { get; set; }

        [RegularExpression(UserValidationConstants.PASSWORD_PATTERN, ErrorMessage = UserValidationConstants.NOT_VALID_PASSWORD)]
        public string? RepeatPassword { get; set; }
    }
}
