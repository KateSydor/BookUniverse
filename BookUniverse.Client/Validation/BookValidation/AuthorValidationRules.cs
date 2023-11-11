namespace BookUniverse.Client.Validation.BookValidation
{
    using System.Globalization;
    using System.Windows.Controls;
    using BookUniverse.DAL.Constants.ValidationConstants;

    public class AuthorValidationRules : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string author = value as string;
            if (author.Length < BookValidationConstants.AUTHOR_MIN_LENGTH || author.Length > BookValidationConstants.AUTHOR_MAX_LENGTH)
            {
                return new ValidationResult(false, $"Author name should be between range {BookValidationConstants.AUTHOR_MIN_LENGTH}-{BookValidationConstants.AUTHOR_MAX_LENGTH}");
            }

            return new ValidationResult(true, null);
        }
    }
}
