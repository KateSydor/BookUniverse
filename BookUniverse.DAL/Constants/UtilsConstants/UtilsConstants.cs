namespace BookUniverse.DAL.Constants.UtilsConstants
{
    public static class UtilsConstants
    {
        public static string FILE_PATH => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "auth.txt");

        public static string FILE_ERROR = "File does not contain necessary information.";
    }
}
