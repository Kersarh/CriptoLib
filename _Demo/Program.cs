using System;
using System.Text;

namespace _Demo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            TestAES();
            TestGOST();
            TestRC4();
            TestRC6();
            TestRSA();
        }



        private static void TestRSA()
        {
            Console.WriteLine("\n----- RSA -----\n");

            string msg = "My Test Message for RSA";
            Console.WriteLine($"Оригинальное сообщение: {msg}");

            // Создаем Экземпляр класса
            LibCriptoRSA.RsaApi criptAlise = new(); // Экземпляр для Алисы
            LibCriptoRSA.RsaApi criptBob = new(); // Экземпляр для Боба
            
            // Пример перехвата уведомлений
            criptAlise.keyService.KeyRewrite += Key_KeyRewrite;
            criptAlise.signatureService.SigNotify += Signature_SigNotify;

            // Создадим пары ключей для Алисы и Боба
            (byte[] aliseKeyPrivate, byte[] aliseKeyPublic) = criptAlise.keyService.CreateKey();
            (byte[] bobKeyPrivate, byte[] bobKeyPublic) = criptBob.keyService.CreateKey();

            // Экспорт ключа
            string a = criptAlise.keyService.ExportPrivateKey(aliseKeyPrivate);
            // Console.WriteLine(a); // Отобразить ключ в консоли

            //Алиса шифрует сообщение для Боба
            string encmsg = criptAlise.encryptService.Encript(aliseKeyPrivate, bobKeyPublic, msg);

            // Боб расшифровывает сообщение Алисы
            (string, bool) mes = criptBob.decryptService.Decript(bobKeyPrivate, aliseKeyPublic, encmsg);
            Console.WriteLine(mes);
        }

        private static bool Signature_SigNotify(bool b)
        {
            Console.WriteLine($"Подпись: {b}");
            return b;
        }

        private static bool Key_KeyRewrite(string path)
        {
            return true;
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

        private static void TestRC4()
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

        private static void TestRC6()
        {
            string data = "Hello RC6!!!";
            string password = "pass";

            Console.WriteLine("\n----- RC6 -----\n");

            LibCriptoRC6.RC6 encRC6 = new(128, password);
            var enc = encRC6.Encrypt(data);
            Console.WriteLine($"Encrypt: \n{Encoding.UTF8.GetString(enc)}\n");

            // Декодирование
            LibCriptoRC6.RC6 decRC6 = new(128, password);
            var dec = decRC6.Decrypt(enc);
            Console.WriteLine($"Decrypt: \n{Encoding.UTF8.GetString(dec)}\n");

            LibCriptoRC6.RC6 decRC6Err = new(128, "fail");
            var decErr = decRC6Err.Decrypt(enc);
            Console.WriteLine($"Decrypt invalid password: \n{Encoding.UTF8.GetString(decErr)}\n");
        }
    }
}