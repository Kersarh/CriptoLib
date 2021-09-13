namespace LibCriptoRSA
{
    /// <summary>
    /// Шифрование данных
    /// </summary>
    public interface IEncryption
    {
        /// <summary>
        /// Шифрует сообщение
        /// </summary>
        /// <param name="alise_private_key"></param>
        /// <param name="bob_public_key"></param>
        /// <param name="message"></param>
        /// <returns>Base64 string</returns>
        public string Encript(byte[] alise_private_key, byte[] bob_public_key, string message);

        /// <summary>
        /// Шифрует файл
        /// </summary>
        /// <param name="alise_private_key"></param>
        /// <param name="bob_public_key"></param>
        /// <param name="file"></param>
        /// <returns>Base64 string</returns>
        public string EncriptFile(byte[] alise_private_key, byte[] bob_public_key, string file);

        /// <summary>
        /// Сохраняет сообщение в файл
        /// </summary>
        /// <param name="message"></param>
        /// <param name="file"></param>
        /// <returns>true/false</returns>
    }
}