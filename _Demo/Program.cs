using System;

namespace _Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            TestAES();
        }

        static void TestAES()
        {
            string data = "Hello AES!!!";
            string password = "pass";
            LibCriptoAES.API ApiAES = new();
            var enc = ApiAES.encrypt.Encrypt(data, password);
            Console.WriteLine(enc);

            var dec = ApiAES.decrypt.Decrypt(enc, password);
            Console.WriteLine(dec);

        }
    }
}
