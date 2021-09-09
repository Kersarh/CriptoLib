using LibCriptoAES.Interface;

namespace LibCriptoAES
{
    public class AesApi
    {
        public IEncryption encrypt;
        public IDecryption decrypt;

        public AesApi()
        {
            encrypt = new EncryptData();
            decrypt = new DecryptData();
        }
    }
}