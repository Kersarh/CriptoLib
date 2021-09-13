using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace LibCriptoRSA
{
    public class EncriptData : IEncryption
    {
        private readonly ISignatureData signature;

        public EncriptData(ISignatureData sig)
        {
            signature = sig;
        }

        public string Encript(byte[] alise_private_key, byte[] bob_public_key, string message)
        {
            using RSACryptoServiceProvider rsaKey = new();
            rsaKey.ImportCspBlob(bob_public_key);

            // Получаем вектор
            using Aes aes = new AesCryptoServiceProvider();
            byte[] iv = aes.IV;

            // Шифруем сессионный ключ
            RSAOAEPKeyExchangeFormatter keyFormatter = new(rsaKey);
            byte[] encryptedSessionKey = keyFormatter.CreateKeyExchange(aes.Key, typeof(Aes));

            // Шифруем сообщение
            using MemoryStream ciphertext = new();
            using CryptoStream cs = new(ciphertext, aes.CreateEncryptor(), CryptoStreamMode.Write);
            byte[] plaintextMessage = Encoding.UTF8.GetBytes(message);
            cs.Write(plaintextMessage, 0, plaintextMessage.Length);
            cs.Close();

            byte[] encryptedMessage = ciphertext.ToArray();

            // Подписываем сообщение
            byte[] sig = signature.SignatureCreate(alise_private_key, plaintextMessage);

            // Компонуем данные в словарь
            Dictionary<string, byte[]> result = new()
            {
                { "iv", iv },
                { "encSessionKey", encryptedSessionKey },
                { "encMessage", encryptedMessage },
                { "sig", sig },
            };

            // преобразуем в -> Json -> строку Base64
            JsonSerializerOptions options = new() { WriteIndented = true };
            string json = JsonSerializer.Serialize(result, options);
            string B64json = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));

            return B64json;
        }

        public string EncriptFile(byte[] alise_private_key, byte[] bob_public_key, string file)
        {
            string data = null;
            try
            {
                using StreamReader sr = new(file);
                data = sr.ReadToEnd();
            }
            catch (Exception)
            {
                return null;
            }

            return Encript(alise_private_key, bob_public_key, data);
        }
    }
}