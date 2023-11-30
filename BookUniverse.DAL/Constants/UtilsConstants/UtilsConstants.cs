namespace BookUniverse.DAL.Constants.UtilsConstants
{
    public static class UtilsConstants
    {
        public static string ERROR = "Error";

        public static string FILE_PATH => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "auth.txt");

        public static string FILE_ERROR = "File does not contain necessary information.";

        public static string INPUT_VALID_DATA = "Please, input valid data";

        public static int BOOKS_PER_PAGE_PAGINATION = 13;
      
        public static string ALREADY_ADDED_BOOK = "Book has already been added";

        public static string SUCCESSFULLY_ADDED_BOOK = "Book has succesffully been added";
    }
}
