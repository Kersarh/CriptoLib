using LibCriptoAES.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LibCriptoAES
{
    class KeyData:IKey
    {

        public byte[] CreateKey()
        {
            using Aes myAes = Aes.Create();
            return myAes.Key;
        }

        public (byte[], byte[]) CreatePassword(string password)
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

        public byte[] UsePassword(string password, byte[] salt)
        {
            Rfc2898DeriveBytes k1 = new(password, salt, 10000);
            byte[] pass = k1.GetBytes(16);

            return pass;
        }

    }
}
