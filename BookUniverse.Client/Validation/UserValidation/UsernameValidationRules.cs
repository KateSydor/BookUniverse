namespace BookUniverse.Client.Validation.UserValidation
{
    using System.Globalization;
    using System.Windows.Controls;

    public class UsernameValidationRules : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string username = value as string;
            if (username.Length < 6 || username.Length > 30)
            {
                return new ValidationResult(false, "Username should be between range 6-30");
            }

            return new ValidationResult(true, null);
        }
    }
}
