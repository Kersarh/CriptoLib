using System;

namespace _Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            TestAES();
            TestGOST();
        }

        private static void TestGOST()
        {
            string data = "Hello GOST!!!";
            string password = "pass";

            LibCriptoGOST.API ApiGOST = new();

            var enc = ApiGOST.encrypt.Encrypt(data, password);
            Console.WriteLine(enc);

            var dec = ApiGOST.decrypt.Decrypt(enc, password);
            Console.WriteLine(dec);

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
