using LibCriptoGOST.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LibCriptoGOST
{
    internal class EncryptData : Base, IEncryption
    {
        public string Encrypt(string msg, string pass, bool isParallel = false)
        {
            (byte[], byte[]) vkey = KeyData.CreatePassword(pass);
            byte[] salt = vkey.Item1;
            byte[] key = vkey.Item2;

            // Если длинна сообщения меньше чем 16 символов дополнить пробелами
            if (msg.Length < 16) msg += new string(' ', 16 - msg.Length);

            byte[] data = Encoding.UTF8.GetBytes(msg);
            uint[] subkeys = KeyData.GenerateKeys(key);
            byte[] criptMsg = new byte[data.Length];
            byte[] block = new byte[8];

            if (isParallel)
            {
                Parallel.For(0, data.Length / 8, i =>
                {
                    Array.Copy(data, 8 * i, block, 0, 8);
                    Array.Copy(EncodeBlock(block, subkeys), 0, criptMsg, 8 * i, 8);
                });
            }
            else
            {
                for (int i = 0; i < data.Length / 8; i++) // N blocks 64bits length.
                {
                    Array.Copy(data, 8 * i, block, 0, 8);
                    Array.Copy(EncodeBlock(block, subkeys), 0, criptMsg, 8 * i, 8);
                }
            }

            Dictionary<string, byte[]> result = new()
            {
                { "msg", criptMsg },
                { "salt", salt },
            };

            JsonSerializerOptions options = new() { WriteIndented = true };
            string json = JsonSerializer.Serialize(result, options);
            string B64json = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));

            return B64json;
        }

        //private byte[] EncodeBlock(byte[] block, uint[] keys)
        //{
        //    // separate on 2 blocks.
        //    uint N1 = BitConverter.ToUInt32(block, 0);
        //    uint N2 = BitConverter.ToUInt32(block, 4);

        //    for (int i = 0; i < 32; i++)
        //    {
        //        int keyIndex = i < 24 ? (i % 8) : (7 - i % 8); // to 24th cycle : 0 to 7; after - 7 to 0;
        //        uint s = (N1 + keys[keyIndex]) % uint.MaxValue; // (N1 + X[i]) mod 2^32
        //        s = Substitution(s); // substitute from box
        //        s = (s << 11) | (s >> 21);
        //        s ^= N2; // ( s + N2 ) mod 2
        //        //N2 = N1;
        //        //N1 = s;
        //        if (i < 31) // last cycle : N1 don't change; N2 = s;
        //        {
        //            N2 = N1;
        //            N1 = s;
        //        }
        //        else
        //        {
        //            N2 = s;
        //        }
        //    }

        //    byte[] output = new byte[8];
        //    byte[] N1buff = BitConverter.GetBytes(N1);
        //    byte[] N2buff = BitConverter.GetBytes(N2);

        //    for (int i = 0; i < 4; i++)
        //    {
        //        output[i] = N1buff[i];
        //        output[4 + i] = N2buff[i];
        //    }
        //    return output;
        //}
    }
}