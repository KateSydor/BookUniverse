namespace BookUniverse.Client.Validation.UserValidation
{
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    public class PasswordValidationRules : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string password = value as string;
            if (password == null) {
                ((TextBlock)Application.Current.MainWindow.FindName("passwordError")).Text = "Fields cannot be empty!";
                ((TextBlock)Application.Current.MainWindow.FindName("passwordError")).Visibility = Visibility.Visible;

                return new ValidationResult(false, "Fields cannot be empty!");
            }
            if (password.Length < 8 || password.Length > 15)
            {
                ((TextBlock)Application.Current.MainWindow.FindName("passwordError")).Text = "Password should be between range 8-15";
                ((TextBlock)Application.Current.MainWindow.FindName("passwordError")).Visibility = Visibility.Visible;

                return new ValidationResult(false, "Password should be between range 8-15");
            }
            else if (!IsValidPasswrod(password))
            {
                ((TextBlock)Application.Current.MainWindow.FindName("passwordError")).Text = "Password should contain at least uppdercase & lowercase letter, digit, special symbol";
                ((TextBlock)Application.Current.MainWindow.FindName("passwordError")).Visibility = Visibility.Visible;
                return new ValidationResult(false, "Password should contain at least uppdercase & lowercase letter, digit, special symbol");
            }

            ((TextBlock)Application.Current.MainWindow.FindName("passwordError")).Visibility = Visibility.Collapsed;


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
