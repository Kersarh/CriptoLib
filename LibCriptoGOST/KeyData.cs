using System;
using System.Security.Cryptography;

namespace LibCriptoGOST
{
    internal static class KeyData
    {
        public static (byte[] salt, byte[] password) CreatePassword(string password)
        {
            byte[] salt = new byte[8];
            using (RNGCryptoServiceProvider rngCsp = new())
            {
                rngCsp.GetBytes(salt);
            }

            Rfc2898DeriveBytes k1 = new(password, salt, 10000);
            byte[] pass = k1.GetBytes(32);

            (byte[] salt, byte[] password) result = (salt, password: pass);
            return result;
        }

        public static byte[] UsePassword(string password, byte[] salt)
        {
            Rfc2898DeriveBytes k1 = new(password, salt, 10000);
            byte[] pass = k1.GetBytes(32);

            return pass;
        }

        public static uint[] GenerateKeys(byte[] key)
        {
            if (key.Length != 32)
            {
                throw new Exception("Wrong Length Key!");
            }

            uint[] subkeys = new uint[8];

            for (int i = 0; i < 8; i++)
            {
                subkeys[i] = BitConverter.ToUInt32(key, 4 * i);
            }

            return subkeys;
        }
    }
}