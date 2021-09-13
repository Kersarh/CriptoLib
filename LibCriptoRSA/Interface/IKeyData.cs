namespace LibCriptoRSA
{
    /// <summary>
    /// Работа с RSA ключами
    /// </summary>
    public interface IKeyData
    {
        public delegate bool KeyHandler(string path);

        /// <summary>
        /// Событие перезапись ключей
        /// </summary>
        public event KeyHandler KeyRewrite;

        /// <summary>
        /// Создает новые ключи RSA
        /// </summary>
        /// <returns>Словарь: {{ "private", privateKey }, { "public", publicKey }} </returns>
        public (byte[] privKey, byte[] pubKey) CreateKey();

        public string ExportPrivateKey(byte[] key);
    }
}