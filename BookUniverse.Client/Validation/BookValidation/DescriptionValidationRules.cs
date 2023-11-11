namespace BookUniverse.Client.Validation.BookValidation
{
    using System.Globalization;
    using System.Windows.Controls;
    using BookUniverse.DAL.Constants.ValidationConstants;

    public class DescriptionValidationRules : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string description = value as string;
            if (description.Length < BookValidationConstants.DESCRIPTION_MIN_LENGTH || description.Length > BookValidationConstants.DESCRIPTION_MAX_LENGTH)
            {
                return new ValidationResult(false, $"Description should be between range {BookValidationConstants.DESCRIPTION_MIN_LENGTH}-{BookValidationConstants.DESCRIPTION_MAX_LENGTH}");
            }

            return new ValidationResult(true, null);
        }
    }
}
