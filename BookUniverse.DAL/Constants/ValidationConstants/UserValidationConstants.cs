namespace BookUniverse.DAL.Constants.ValidationConstants
{
    public static class UserValidationConstants
    {
        public const string NOT_EMPTY_FIELDS = "Fields cannot be empty!";

        public const int USERNAME_MIN_LENGTH = 6;
        public const int USERNAME_MAX_LENGTH = 30;

        public const int EMAIL_MIN_LENGTH = 15;
        public const int EMAIL_MAX_LENGTH = 256;
        public const string EMAIL_INCORRECT = "Incorrect email";
        public const string EMAIL_PATTERN = @"^[a-zA-Z0-9.]{3,20}@(?:[a-zA-Z0-9]{2,20}\.){1,30}[a-zA-Z]{2,10}$";
        public const string NOT_VALID_EMAIL = "Not valid email.";

        public const int PASSWORD_MAX_LENGTH = 256;
        public const int PASSWORD_DTO_MIN_LENGTH = 8;
        public const int PASSWORD_DTO_MAX_LENGTH = 15;
        public const string PASSWORD_CRITERIA = "At least 1 uppdercase & lowercase letter and digit";
        public const string PASSWORD_PATTERN = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[A-Za-z\d]{8,15}$";
        public const string NOT_VALID_PASSWORD = "Not valid password.";

    }
}
