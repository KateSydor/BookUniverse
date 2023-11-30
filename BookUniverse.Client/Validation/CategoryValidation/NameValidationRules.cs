namespace BookUniverse.Client.Validation.CategoryValidation
{
    using System.Globalization;
    using System.Windows.Controls;
    using BookUniverse.DAL.Constants.ValidationConstants;

    public class NameValidationRules : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string name = value as string;
            if (name.Length < CategoryValidationConstants.CATEGORYNAME_MIN_LENGTH || name.Length > CategoryValidationConstants.CATEGORYNAME_MAX_LENGTH)
            {
                return new ValidationResult(false, $"Category should be between range {CategoryValidationConstants.CATEGORYNAME_MIN_LENGTH}-{CategoryValidationConstants.CATEGORYNAME_MAX_LENGTH}");
            }

            return new ValidationResult(true, null);
        }
    }
}