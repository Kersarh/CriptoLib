namespace LibCriptoRSA
{
    /// <summary>
    /// Расшифровка данных
    /// </summary>
    public interface IDecryption
    {
        /// <summary>
        /// Расшифровывает сообщение
        /// </summary>
        /// <param name="bob_private_key"></param>
        /// <param name="alise_public_key"></param>
        /// <param name="encmsg"></param>
        /// <returns>(string, bool) (Сообщение, верификация подписи) </returns>
        (string, bool) Decript(byte[] bob_private_key, byte[] alise_public_key, string encmsg);

        /// <summary>
        /// Расшифровывает файл
        /// </summary>
        /// <param name="bob_private_key"></param>
        /// <param name="alise_public_key"></param>
        /// <param name="file"></param>
        /// <returns>(string, bool) (Сообщение, верификация подписи) </returns>
        (string, bool) DecriptFile(byte[] bob_private_key, byte[] alise_public_key, string file);

        /// <summary>
        /// Сохраняет расшифрованный файл
        /// </summary>
        /// <param name="mes"></param>
        /// <param name="file"></param>
        /// <returns>true/false</returns>
    }
}