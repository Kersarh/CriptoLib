namespace LibCriptoRSA
{
    /// <summary>
    /// Создает и проверяем подпись
    /// </summary>
    public interface ISignatureData
    {
        public delegate bool SigHandler(bool b);

        /// <summary>
        /// Событие проверки подписи
        /// </summary>
        public event SigHandler SigNotify;

        /// <summary>
        /// Проверяет подпись
        /// </summary>
        /// <param name="private_key"></param>
        /// <param name="message"></param>
        /// <returns>byte[] signature</returns>
        public byte[] SignatureCreate(byte[] private_key, byte[] message);

        /// <summary>
        /// Создает подпись
        /// </summary>
        /// <param name="public_key"></param>
        /// <param name="message"></param>
        /// <param name="signature"></param>
        /// <returns>true/false</returns>
        public bool SignatureCheck(byte[] public_key, byte[] message, byte[] signature);
    }
}