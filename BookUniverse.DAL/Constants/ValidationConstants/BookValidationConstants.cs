namespace BookUniverse.DAL.Constants.ValidationConstants
{
    public static class BookValidationConstants
    {
        public const int BOOKTITLE_MIN_LENGTH = 3;
        public const int BOOKTITLE_MAX_LENGTH = 100;

        public const int AUTHOR_MIN_LENGTH = 5;
        public const int AUTHOR_MAX_LENGTH = 50;

        public const int DESCRIPTION_MIN_LENGTH = 30;
        public const int DESCRIPTION_MAX_LENGTH = 300;

        public const int PAGES_MIN = 1;
        public const int PAGES_MAX = 1200;

        public const double RATE_MIN = 0.0;
        public const double RATE_MAX = 5.0;
    }
}
