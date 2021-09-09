using LibCriptoGOST.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LibCriptoGOST
{
    internal class DecryptData : Base, IDecryption
    {
        public string Decrypt(string encmsg, string pass, bool isParallel = false)
        {
            string decData = Encoding.UTF8.GetString(Convert.FromBase64String(encmsg));
            Dictionary<string, byte[]> msg = JsonSerializer.Deserialize<Dictionary<string, byte[]>>(decData);

            byte[] data = msg["msg"];
            byte[] salt = msg["salt"];
            byte[] key = KeyData.UsePassword(pass, salt);

            uint[] subkeys = KeyData.GenerateKeys(key);
            byte[] decriptMsg = new byte[data.Length];
            byte[] block = new byte[8];

            if (isParallel)
            {
                Parallel.For(0, data.Length / 8, i =>
                {
                    Array.Copy(data, 8 * i, block, 0, 8);
                    Array.Copy(DecodeBlock(block, subkeys), 0, decriptMsg, 8 * i, 8);
                });
            }
            else
            {
                for (int i = 0; i < data.Length / 8; i++)
                {
                    Array.Copy(data, 8 * i, block, 0, 8);
                    Array.Copy(DecodeBlock(block, subkeys), 0, decriptMsg, 8 * i, 8);
                }
            }

            string result = Encoding.UTF8.GetString(decriptMsg);
            return result;
        }
    }
}