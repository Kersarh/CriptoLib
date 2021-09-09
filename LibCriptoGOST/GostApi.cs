using LibCriptoGOST.Interface;
using System;

namespace LibCriptoGOST
{
    public class GostApi
    {

        public IEncryption encrypt;
        public IDecryption decrypt;

        public GostApi()
        {
            encrypt = new EncryptData();
            decrypt = new DecryptData();
        }
    }
}
