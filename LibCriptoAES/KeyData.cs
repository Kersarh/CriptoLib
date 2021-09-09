using System.Security.Cryptography;

namespace LibCriptoAES
{
    internal static class KeyData
    {
        public static (byte[], byte[]) CreatePassword(string password)
        {
            // Соль
            byte[] salt = new byte[8];
            using (RNGCryptoServiceProvider rngCsp = new())
            {
                rngCsp.GetBytes(salt);
            }

            Rfc2898DeriveBytes k1 = new(password, salt, 10000);
            byte[] pass = k1.GetBytes(16);

            (byte[] salt, byte[] password) result = (salt, password: pass);
            return result;
        }

        public static byte[] UsePassword(string password, byte[] salt)
        {
            Rfc2898DeriveBytes k1 = new(password, salt, 10000);
            byte[] pass = k1.GetBytes(16);

            return pass;
        }
    }
}