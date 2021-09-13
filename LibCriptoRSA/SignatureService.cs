using System.Security.Cryptography;

namespace LibCriptoRSA
{
    public class SignatureService : ISignatureData
    {
        public event ISignatureData.SigHandler SigNotify;

        public bool SignatureCheck(byte[] public_key, byte[] message, byte[] signature)
        {
            // данные подписываются приватным ключом
            // а расшифровываются публичным

            // Создайте новый экземпляр RSA
            using RSACryptoServiceProvider rsa = new();
            rsa.ImportCspBlob(public_key);
            // Хеш для подписи.
            byte[] hash;
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] data = message;
                hash = sha256.ComputeHash(data);
            }
            // Создаем объект RSAPKCS1SignatureDeformatter и передаем ему
            // Экземпляр RSA для передачи ключевой информации.
            RSAPKCS1SignatureDeformatter RSADeformatter = new(rsa);
            RSADeformatter.SetHashAlgorithm("SHA256");
            // Проверяем хеш и выводим результаты на консоль.
            if (RSADeformatter.VerifySignature(hash, signature))
            {
                // Сигнатуры совпадают
                SigNotify?.Invoke(true);
                return true;
            }
            else
            {
                // Ошибка!!!
                SigNotify?.Invoke(false);
                return false;
            }
        }

        public byte[] SignatureCreate(byte[] private_key, byte[] message)
        {
            // Данные подписываются приватным ключом
            // а расшифровываются публичным

            // Создайте экземпляр RSA
            using RSACryptoServiceProvider rsaKey = new();
            rsaKey.ImportCspBlob(private_key);

            // Hash для подписи
            byte[] hash;
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] data = message;
                hash = sha256.ComputeHash(data);
            }

            // Создаем объект RSASignatureFormatter и передаем ему
            // Экземпляр RSA для передачи ключевой информации.
            RSAPKCS1SignatureFormatter RSAFormatter = new(rsaKey);

            // Устанавливаем алгоритм хеширования SHA256.
            RSAFormatter.SetHashAlgorithm("SHA256");

            // Создаем подпись для HashValue и возвращаем ее.
            byte[] SignedHash = RSAFormatter.CreateSignature(hash);

            return SignedHash;
        }
    }
}