namespace BookUniverse.Client.Validation.UserValidation
{
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Windows.Controls;

    public class EmailValidationRules : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string email = value as string;
            if (email.Length < 15 || email.Length > 256)
            {
                return new ValidationResult(false, "Email should be between range 15-256");
            }
            else if (!IsValidEmail(email))
            {
                return new ValidationResult(false, "Incorrect email");
            }

            return new ValidationResult(true, null);
        }

        private static bool IsValidEmail(string value)
        {
            bool check = Regex.
                IsMatch(value, @"^[a-zA-Z0-9.]{3,20}@(?:[a-zA-Z0-9]{2,20}\.){1,30}[a-zA-Z]{2,10}$");
            if (!check)
            {
                return false;
            }

            return true;
        }
    }
}
