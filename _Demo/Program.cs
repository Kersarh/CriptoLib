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

            LibCriptoGOST.GostApi ApiGOST = new();
                        var enc = ApiGOST.encrypt.Encrypt(data, password);
            Console.WriteLine(enc);

            LibCriptoGOST.GostApi ApiGOST2 = new();
            var dec = ApiGOST2.decrypt.Decrypt(enc, password);
            Console.WriteLine(dec);

            // Ввод ошибочного пароля
            var decErr = ApiGOST2.decrypt.Decrypt(enc, "fail");
            Console.WriteLine(decErr);

        }

        static void TestAES()
        {
            string data = "Hello AES!!!";
            string password = "pass";
            LibCriptoAES.AesApi ApiAES = new();

            var enc = ApiAES.encrypt.Encrypt(data, password);
            Console.WriteLine(enc);

            LibCriptoAES.AesApi ApiAES2 = new();
            var dec = ApiAES2.decrypt.Decrypt(enc, password);
            Console.WriteLine(dec);

            var decErr = ApiAES2.decrypt.Decrypt(enc, "fail");
            Console.WriteLine(decErr);
        }
    }
}
