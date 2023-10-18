namespace BookUniverse.BLL.Utils
{
    using System.Security.Cryptography;
    using System.Text;

    public static class Hasher
    {
        public static string ComputeHash(string password)
        {
            string hashed = "";
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                hashed = builder.ToString();
            }
            return hashed;
        }
    }
}
