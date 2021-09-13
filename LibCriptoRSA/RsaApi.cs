namespace LibCriptoRSA

{
    /// <summary>
    /// Внешний интерфейс для работы RSA
    /// </summary>
    public class RsaApi
    {
        public IKeyData keyService;
        public ISignatureData signatureService;
        public IEncryption encryptService;
        public IDecryption decryptService;

        public RsaApi()
        {
            keyService = new KeyData();
            signatureService = new SignatureService();
            encryptService = new EncriptData(signatureService);
            decryptService = new DecriptData(signatureService);
        }
    }
}