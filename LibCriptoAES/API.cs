using LibCriptoAES.Interface;
using System;

namespace LibCriptoAES
{
    public class API
    {
        public IKey key;
        public IEncryption encrypt;
        public IDecryption decrypt;

        public API()
        {
            key = new KeyData();
            encrypt = new EncryptData(key);
            decrypt = new DecryptData(key);
        }
    }
}
