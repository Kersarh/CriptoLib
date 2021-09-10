using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibCriptoRC4
{
    public class Base
    {
        public byte[] Password { get; set; }

        public Base(string password)
        {
            byte[] passwordByte = Encoding.UTF8.GetBytes(password);
            Password = passwordByte;
        }

        /// <summary>
        /// Encrypt String
        /// </summary>
        /// <param name="password"></param>
        /// <param name="data"></param>
        /// <returns>string</returns>
        public string Encrypt(string data)
        {
            byte[] dataByte = Encoding.UTF8.GetBytes(data);

            byte[] res = Encrypt(dataByte);
            return Convert.ToBase64String(res);
        }

        /// <summary>
        /// Encrypt byte[]
        /// </summary>
        /// <param name="Password"></param>
        /// <param name="data"></param>
        /// <returns>byte[]</returns>
        public byte[] Encrypt(byte[] data)
        {
            int a, i, j, k, tmp;
            int[] key, box;
            byte[] cipher;

            key = new int[256];
            box = new int[256];
            cipher = new byte[data.Length];

            for (i = 0; i < 256; i++)
            {
                key[i] = Password[i % Password.Length];
                box[i] = i;
            }
            for (j = i = 0; i < 256; i++)
            {
                j = (j + box[i] + key[i]) % 256;
                tmp = box[i];
                box[i] = box[j];
                box[j] = tmp;
            }
            for (a = j = i = 0; i < data.Length; i++)
            {
                a++;
                a %= 256;
                j += box[a];
                j %= 256;
                tmp = box[a];
                box[a] = box[j];
                box[j] = tmp;
                k = box[((box[a] + box[j]) % 256)];
                cipher[i] = (byte)(data[i] ^ k);
            }
            return cipher;
        }

        /// <summary>
        /// Decrypt String
        /// </summary>
        /// <param name="password"></param>
        /// <param name="data"></param>
        /// <returns>string</returns>
        public string Decrypt(string data)
        {
            byte[] dataByte = Convert.FromBase64String(data);

            byte[] result = Encrypt(dataByte);
            return Encoding.UTF8.GetString(result);
        }

        /// <summary>
        /// Decrypt byte[]
        /// </summary>
        /// <param name="password"></param>
        /// <param name="data"></param>
        /// <returns>byte[]</returns>
        public byte[] Decrypt(byte[] data)
        {
            return Encrypt(data);
        }

    }
}
