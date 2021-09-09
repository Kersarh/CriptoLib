using LibCriptoGOST.Interface;
using System;

namespace LibCriptoGOST
{
    public class API
    {

        public IEncryption encrypt;
        public IDecryption decrypt;

        public API()
        {
            encrypt = new EncryptData();
            decrypt = new DecryptData();
        }
    }
}
