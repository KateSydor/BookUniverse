namespace BookUniverse.BLL.DTOs
{
    using System.ComponentModel.DataAnnotations;
    using BookUniverse.DAL.Constants.ValidationConstants;

    public class LoginDto
    {
        public string? Username { get; set; }

        [RegularExpression(UserValidationConstants.PASSWORD_PATTERN, ErrorMessage = UserValidationConstants.NOT_VALID_PASSWORD)]
        public string? Password { get; set; }
    }
}
