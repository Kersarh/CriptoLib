using LibCriptoAES.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LibCriptoAES
{
    class EncryptData:IEncryption
    {
        public IKey keyService;

        public EncryptData(IKey key)
        {
            keyService = key;
        }

        public string Encrypt(string message, string pass)
        {
            (byte[], byte[]) vkey = keyService.CreatePassword(pass);
            byte[] salt = vkey.Item1;
            byte[] key = vkey.Item2;

            // Создаем объект и генерируем начальный вектор
            using Aes aesAlg = Aes.Create();
            aesAlg.Key = key;

            // создаем потоки для шифрования
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            using MemoryStream msEncrypt = new();
            using CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write);
            using (StreamWriter swEncrypt = new(csEncrypt))
            {
                swEncrypt.Write(message);
            }

            byte[] encrypted;
            encrypted = msEncrypt.ToArray();

            Dictionary<string, byte[]> result = new()
            {
                { "IV", aesAlg.IV },
                { "encMessage", encrypted },
                { "salt", salt },
            };

            JsonSerializerOptions options = new() { WriteIndented = true };
            string json = JsonSerializer.Serialize(result, options);
            string B64json = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));

            return B64json;
        }
    }
}
