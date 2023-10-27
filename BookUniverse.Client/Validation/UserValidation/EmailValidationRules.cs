namespace BookUniverse.Client.Validation.UserValidation
{
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Windows.Controls;
    using BookUniverse.DAL.Constants.ValidationConstants;

    public class EmailValidationRules : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string email = value as string;
            if (email.Length < UserValidationConstants.EMAIL_MIN_LENGTH || email.Length > UserValidationConstants.EMAIL_MAX_LENGTH)
            {
                return new ValidationResult(false, $"Email should be between range {UserValidationConstants.EMAIL_MIN_LENGTH}-{UserValidationConstants.EMAIL_MAX_LENGTH}");
            }
            else if (!IsValidEmail(email))
            {
                return new ValidationResult(false, UserValidationConstants.EMAIL_INCORRECT);
            }

            return new ValidationResult(true, null);
        }

        private static bool IsValidEmail(string value)
        {
            return Regex.IsMatch(value, UserValidationConstants.EMAIL_PATTERN);
        }
    }
}
