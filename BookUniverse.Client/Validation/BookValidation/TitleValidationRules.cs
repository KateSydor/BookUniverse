namespace BookUniverse.Client.Validation.BookValidation
{
    using System.Globalization;
    using System.Windows.Controls;
    using BookUniverse.DAL.Constants.ValidationConstants;

    public class TitleValidationRules : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string title = value as string;
            if (title.Length < BookValidationConstants.BOOKTITLE_MIN_LENGTH || title.Length > BookValidationConstants.BOOKTITLE_MAX_LENGTH)
            {
                return new ValidationResult(false, $"Title should be between range {BookValidationConstants.BOOKTITLE_MIN_LENGTH}-{BookValidationConstants.BOOKTITLE_MAX_LENGTH}");
            }

            return new ValidationResult(true, null);
        }
    }
}
