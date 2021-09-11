using System;

namespace _Demo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            TestAES();
            TestGOST();
            TestRC4();
        }

        private static void TestGOST()
        {
            string data = "Hello GOST!!!";
            string password = "pass";

            Console.WriteLine("\n----- GOST -----\n");

            LibCriptoGOST.GostApi ApiGOST = new();
            var enc = ApiGOST.encrypt.Encrypt(data, password);
            Console.WriteLine($"Encrypt: \n{enc}\n");

            LibCriptoGOST.GostApi ApiGOST2 = new();
            var dec = ApiGOST2.decrypt.Decrypt(enc, password);
            Console.WriteLine($"Decrypt: \n{dec}\n");

            // Ввод ошибочного пароля
            var decErr = ApiGOST2.decrypt.Decrypt(enc, "fail");
            Console.WriteLine($"Decrypt invalid password: \n{decErr}\n");
        }

        private static void TestAES()
        {
            string data = "Hello AES!!!";
            string password = "pass";

            Console.WriteLine("\n----- AES -----\n");

            LibCriptoAES.AesApi ApiAES = new();
            var enc = ApiAES.encrypt.Encrypt(data, password);
            Console.WriteLine($"Encrypt: \n{enc}\n");

            LibCriptoAES.AesApi ApiAES2 = new();
            var dec = ApiAES2.decrypt.Decrypt(enc, password);
            Console.WriteLine($"Decrypt: \n{dec}\n");

            var decErr = ApiAES2.decrypt.Decrypt(enc, "fail");
            Console.WriteLine($"Decrypt invalid password: \n{decErr}\n");
        }

        static void TestRC4()
        {
            string data = "Hello RC4!!!";
            string password = "pass";

            Console.WriteLine("\n----- RC4 -----\n");

            LibCriptoRC4.Base rc4 = new();
            string enc = rc4.Encrypt(data, password); 
            Console.WriteLine($"Encrypt: \n{enc}\n");

            LibCriptoRC4.Base rc4_2 = new();
            var dec = rc4_2.Decrypt(enc, password);
            Console.WriteLine($"Decrypt: \n{dec}\n");

            var decErr = rc4_2.Decrypt(enc, "fail");
            Console.WriteLine($"Decrypt invalid password: \n{decErr}\n");
        }
    }
}