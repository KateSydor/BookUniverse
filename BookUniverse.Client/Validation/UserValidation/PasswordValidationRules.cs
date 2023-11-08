namespace BookUniverse.Client.Validation.UserValidation
{
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Controls;
    using BookUniverse.DAL.Constants.ValidationConstants;

    public class PasswordValidationRules : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string password = value as string;
            if (password == null)
            {
                SetError(UserValidationConstants.NOT_EMPTY_FIELDS);
                return new ValidationResult(false, UserValidationConstants.NOT_EMPTY_FIELDS);
            }

            if (password.Length < UserValidationConstants.PASSWORD_DTO_MIN_LENGTH || password.Length > UserValidationConstants.PASSWORD_DTO_MAX_LENGTH)
            {
                string errorMessage = $"Password should be between range {UserValidationConstants.PASSWORD_DTO_MIN_LENGTH}-{UserValidationConstants.PASSWORD_DTO_MAX_LENGTH}";
                SetError(errorMessage);
                return new ValidationResult(false, $"Password should be between range {UserValidationConstants.PASSWORD_DTO_MIN_LENGTH}-{UserValidationConstants.PASSWORD_DTO_MAX_LENGTH}");
            }
            else if (!IsValidPasswrod(password))
            {
                SetError(UserValidationConstants.PASSWORD_CRITERIA);
                return new ValidationResult(false, UserValidationConstants.PASSWORD_CRITERIA);
            }

            ClearError();
            return new ValidationResult(true, null);
        }

        private void SetError(string errorMessage)
        {
            TextBlock passwordError = Application.Current.MainWindow?.FindName("passwordError") as TextBlock;
            if (passwordError != null)
            {
                passwordError.Text = errorMessage;
                passwordError.Visibility = Visibility.Visible;
            }
        }

        private void ClearError()
        {
            TextBlock passwordError = Application.Current.MainWindow.FindName("passwordError") as TextBlock;
            if (passwordError != null)
            {
                passwordError.Visibility = Visibility.Collapsed;
            }
        }

        private static bool IsValidPasswrod(string value)
        {
            return Regex.IsMatch(value, UserValidationConstants.PASSWORD_PATTERN);
        }
    }
}
