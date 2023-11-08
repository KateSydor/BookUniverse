namespace BookUniverse.BLL.DTOs
{
    using System.ComponentModel.DataAnnotations;
    using BookUniverse.DAL.Constants.ValidationConstants;

    public class EditUserDto
    {
        public string? Username { get; set; }

        [RegularExpression(UserValidationConstants.EMAIL_PATTERN, ErrorMessage = UserValidationConstants.NOT_VALID_EMAIL)]
        public string? Email { get; set; }
    }
}