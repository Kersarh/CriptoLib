using System;
using System.Security.Cryptography;
using System.Text;

namespace LibCriptoRC6
{
    public class RC6
    {
        private const int R = 20;
        private readonly uint[] RoundKey = new uint[2 * R + 4];
        private const int W = 32;
        private byte[] MainKey;
        private const uint P32 = 0xB7E15163;
        private const uint Q32 = 0x9E3779B9;

        public RC6(int keyLong, string key)
        {
            GenerateKey(keyLong, key);
        }

        private void GenerateKey(int Long, string pass)
        {
            MainKey = CreateKeyFromPassword(16, pass);

            int i, j;
            var c = Long switch
            {
                128 => 4, // длина ключа => кол-во слов в ключе
                192 => 6,
                256 => 8,
                _ => 4, // default
            };

            uint[] L = new uint[c];
            for (i = 0; i < c; i++)
            {
                L[i] = BitConverter.ToUInt32(MainKey, i * 4);
            }
            // Генерация RoundKey
            RoundKey[0] = P32;
            for (i = 1; i < 2 * R + 4; i++)
                RoundKey[i] = RoundKey[i - 1] + Q32;
            uint A, B;
            A = B = 0;
            i = j = 0;
            int V = 3 * Math.Max(c, 2 * R + 4);
            for (int s = 1; s <= V; s++)
            {
                A = RoundKey[i] = LeftShift((RoundKey[i] + A + B), 3);
                B = L[j] = LeftShift((L[j] + A + B), (int)(A + B));
                i = (i + 1) % (2 * R + 4);
                j = (j + 1) % c;
            }
        }

        public static byte[] CreateKeyFromPassword(int lenPass, string password)
        {
            byte[] salt = new byte[8] { 1, 1, 1, 1, 1, 1, 1, 1 };
            Rfc2898DeriveBytes k1 = new(password, salt, 10);
            byte[] pass = k1.GetBytes(lenPass);

            return pass;
        }

        // Сдвиг вправо
        private static uint RightShift(uint value, int shift)
        {
            return (value >> shift) | (value << (W - shift));
        }

        //Сдвиг влево
        private static uint LeftShift(uint value, int shift)
        {
            return (value << shift) | (value >> (W - shift));
        }

        private static byte[] ToArrayBytes(uint[] uints, int Long)
        {
            byte[] arrayBytes = new byte[Long * 4];
            for (int i = 0; i < Long; i++)
            {
                byte[] temp = BitConverter.GetBytes(uints[i]);
                temp.CopyTo(arrayBytes, i * 4);
            }
            return arrayBytes;
        }

        public byte[] Encrypt(string plaintext)
        {
            uint A, B, C, D;
            //Преобразование полученного текста в массив байт
            byte[] byteText = Encoding.UTF8.GetBytes(plaintext);
            int i = byteText.Length;
            while (i % 16 != 0)
                i++;

            byte[] text = new byte[i];
            byteText.CopyTo(text, 0);
            byte[] cipherText = new byte[i];
            for (i = 0; i < text.Length; i += 16)
            {
                A = BitConverter.ToUInt32(text, i);
                B = BitConverter.ToUInt32(text, i + 4);
                C = BitConverter.ToUInt32(text, i + 8);
                D = BitConverter.ToUInt32(text, i + 12);
                B += RoundKey[0];
                D += RoundKey[1];
                for (int j = 1; j <= R; j++)
                {
                    uint t = LeftShift((B * (2 * B + 1)), (int)(Math.Log(W, 2)));
                    uint u = LeftShift((D * (2 * D + 1)), (int)(Math.Log(W, 2)));
                    A = (LeftShift((A ^ t), (int)u)) + RoundKey[j * 2];
                    C = (LeftShift((C ^ u), (int)t)) + RoundKey[j * 2 + 1];
                    uint temp = A;
                    A = B;
                    B = C;
                    C = D;
                    D = temp;
                }
                A += RoundKey[2 * R + 2];
                C += RoundKey[2 * R + 3];
                uint[] tempWords = new uint[4] { A, B, C, D };
                byte[] block = ToArrayBytes(tempWords, 4);
                block.CopyTo(cipherText, i);
            }
            return cipherText;
        }

        public byte[] Decrypt(byte[] cipherText)
        {
            uint A, B, C, D;
            int i;
            byte[] plainText = new byte[cipherText.Length];
            for (i = 0; i < cipherText.Length; i += 16)
            {
                A = BitConverter.ToUInt32(cipherText, i);
                B = BitConverter.ToUInt32(cipherText, i + 4);
                C = BitConverter.ToUInt32(cipherText, i + 8);
                D = BitConverter.ToUInt32(cipherText, i + 12);
                C -= RoundKey[2 * R + 3];
                A -= RoundKey[2 * R + 2];
                for (int j = R; j >= 1; j--)
                {
                    uint temp = D;
                    D = C;
                    C = B;
                    B = A;
                    A = temp;
                    uint u = LeftShift((D * (2 * D + 1)), (int)Math.Log(W, 2));
                    uint t = LeftShift((B * (2 * B + 1)), (int)Math.Log(W, 2));
                    C = RightShift((C - RoundKey[2 * j + 1]), (int)t) ^ u;
                    A = RightShift((A - RoundKey[2 * j]), (int)u) ^ t;
                }
                D -= RoundKey[1];
                B -= RoundKey[0];
                uint[] tempWords = new uint[4] { A, B, C, D };
                byte[] block = ToArrayBytes(tempWords, 4);
                block.CopyTo(plainText, i);
            }
            return plainText;
        }
    }
}