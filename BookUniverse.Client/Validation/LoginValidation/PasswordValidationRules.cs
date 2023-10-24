namespace BookUniverse.Client.Validation.LoginValidation
{
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Windows.Controls;

    public class PasswordValidationRules : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string password = value as string;
            if (password.Length < 8 || password.Length > 15)
            {
                return new ValidationResult(false, "Password should be between range 8-15");
            }
            else if (!IsValidPasswrod(password))
            {
                return new ValidationResult(false, "Password should contain at least uppdercase & lowercase letter, digit, special symbol");
            }

            return new ValidationResult(true, null);
        }

        private static bool IsValidPasswrod(string value)
        {
            bool check = Regex.
                IsMatch(value, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[A-Za-z\d]{8,15}$");
            if (!check)
            {
                return false;
            }

            return true;
        }
    }
}
