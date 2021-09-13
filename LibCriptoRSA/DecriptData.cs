using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace LibCriptoRSA
{
    public class DecriptData : IDecryption
    {
        private readonly ISignatureData signature;

        public DecriptData(ISignatureData sig)
        {
            signature = sig;
        }

        public (string, bool) Decript(byte[] bob_private_key, byte[] alise_public_key, string encmsg)
        {
            try
            {
                string decData = Encoding.UTF8.GetString(Convert.FromBase64String(encmsg));
                Dictionary<string, byte[]> msg = JsonSerializer.Deserialize<Dictionary<string, byte[]>>(decData);

                using RSACryptoServiceProvider rsaKey = new();
                rsaKey.ImportCspBlob(bob_private_key);

                using Aes aes = new AesCryptoServiceProvider
                {
                    IV = msg["iv"]
                };

                // Расшифровываем сессионный ключ
                RSAOAEPKeyExchangeDeformatter keyDeformatter = new(rsaKey);

                aes.Key = keyDeformatter.DecryptKeyExchange(msg["encSessionKey"]);

                // Расшифровываем сообщение
                byte[] message = msg["encMessage"];
                using MemoryStream plaintext = new();
                using CryptoStream cs = new(plaintext, aes.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(message, 0, message.Length);
                cs.Close();

                string result = Encoding.UTF8.GetString(plaintext.ToArray());

                // Проверяем подпись
                byte[] textMessage = Encoding.UTF8.GetBytes(result);
                bool chesk = signature.SignatureCheck(alise_public_key, textMessage, msg["sig"]);

                List<object> res = new();
                res.Add(result);
                res.Add(chesk);

                return (result, chesk);
            }
            catch (Exception) { return (null, false); }
        }

        public (string, bool) DecriptFile(byte[] bob_private_key, byte[] alise_public_key, string file)
        {
            string data = null;
            try
            {
                using StreamReader sr = new(file);
                data = sr.ReadToEnd();
            }
            catch (Exception)
            {
                return (null, false);
            }

            return Decript(bob_private_key, alise_public_key, data);
        }
    }
}