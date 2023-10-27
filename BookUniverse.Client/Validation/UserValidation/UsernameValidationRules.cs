namespace BookUniverse.Client.Validation.UserValidation
{
    using System.Globalization;
    using System.Windows.Controls;
    using BookUniverse.DAL.Constants.ValidationConstants;

    public class UsernameValidationRules : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string username = value as string;
            if (username.Length < UserValidationConstants.USERNAME_MIN_LENGTH || username.Length > UserValidationConstants.USERNAME_MAX_LENGTH)
            {
                return new ValidationResult(false, $"Username should be between range {UserValidationConstants.USERNAME_MIN_LENGTH}-{UserValidationConstants.USERNAME_MAX_LENGTH}");
            }

            return new ValidationResult(true, null);
        }
    }
}
