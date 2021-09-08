﻿using LibCriptoAES.Interface;
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
    class DecryptData:IDecryption
    {
        public IKey keyService;

        public DecryptData(IKey key)
        {
            keyService = key;
        }

        public string Decrypt(string encmsg, string pass)
        {
            string decData = Encoding.UTF8.GetString(Convert.FromBase64String(encmsg));
            Dictionary<string, byte[]> msg = JsonSerializer.Deserialize<Dictionary<string, byte[]>>(decData);

            byte[] salt = msg["salt"];
            byte[] key = keyService.UsePassword(pass, salt);

            string plaintext = null;

            // Создаем объект и генерируем начальный вектор
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = msg["IV"];

                // создаем потоки для шифрования
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using MemoryStream msDecrypt = new(msg["encMessage"]);
                using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
                using StreamReader srDecrypt = new(csDecrypt);
                try
                {
                    plaintext = srDecrypt.ReadToEnd();
                }
                catch (Exception)
                {
                    // WARNING!!!! В случае ошибки (неверный пароль) вернуть пустую строку!
                    plaintext = null;
                }

            }
            return plaintext;
        }
    }
}
