using System;

namespace LibCriptoGOST
{
    internal class Base
    {
        protected byte[][] SubstitutionBox =
        {
              new byte[] { 0x0,0x1,0x2,0x3,0x4,0x5,0x6,0x7,0x8,0x9,0xA,0xB,0xC,0xD,0xE,0xF },
              new byte[] { 0x0,0x1,0x2,0x3,0x4,0x5,0x6,0x7,0x8,0x9,0xA,0xB,0xC,0xD,0xE,0xF },
              new byte[] { 0x0,0x1,0x2,0x3,0x4,0x5,0x6,0x7,0x8,0x9,0xA,0xB,0xC,0xD,0xE,0xF },
              new byte[] { 0x0,0x1,0x2,0x3,0x4,0x5,0x6,0x7,0x8,0x9,0xA,0xB,0xC,0xD,0xE,0xF },
              new byte[] { 0x0,0x1,0x2,0x3,0x4,0x5,0x6,0x7,0x8,0x9,0xA,0xB,0xC,0xD,0xE,0xF },
              new byte[] { 0x0,0x1,0x2,0x3,0x4,0x5,0x6,0x7,0x8,0x9,0xA,0xB,0xC,0xD,0xE,0xF },
              new byte[] { 0x0,0x1,0x2,0x3,0x4,0x5,0x6,0x7,0x8,0x9,0xA,0xB,0xC,0xD,0xE,0xF },
              new byte[] { 0x0,0x1,0x2,0x3,0x4,0x5,0x6,0x7,0x8,0x9,0xA,0xB,0xC,0xD,0xE,0xF }
        };

        protected uint Substitution(uint value)
        {
            uint output = 0;

            for (int i = 0; i < 8; i++)
            {
                byte temp = (byte)((value >> (4 * i)) & 0x0f);
                temp = SubstitutionBox[i][temp];
                output |= (uint)temp << (4 * i);
            }

            return output;
        }

        public byte[] EncodeBlock(byte[] block, uint[] keys)
        {
            uint N1 = BitConverter.ToUInt32(block, 0);
            uint N2 = BitConverter.ToUInt32(block, 4);

            for (int i = 0; i < 32; i++)
            {
                int keyIndex = i < 24 ? (i % 8) : (7 - i % 8);
                uint s = (N1 + keys[keyIndex]) % uint.MaxValue;
                s = Substitution(s);
                s = (s << 11) | (s >> 21);
                s ^= N2;

                if (i < 31)
                {
                    N2 = N1;
                    N1 = s;
                }
                else
                {
                    N2 = s;
                }
            }

            byte[] output = new byte[8];
            byte[] N1buff = BitConverter.GetBytes(N1);
            byte[] N2buff = BitConverter.GetBytes(N2);

            for (int i = 0; i < 4; i++)
            {
                output[i] = N1buff[i];
                output[4 + i] = N2buff[i];
            }
            return output;
        }

        public byte[] DecodeBlock(byte[] block, uint[] keys)
        {
            uint N1 = BitConverter.ToUInt32(block, 0);
            uint N2 = BitConverter.ToUInt32(block, 4);

            for (int i = 0; i < 32; i++)
            {
                int keyIndex = (i < 8) ? (i % 8) : (7 - i % 8);
                uint s = (N1 + keys[keyIndex]) % uint.MaxValue;
                s = Substitution(s);
                s = (s << 11) | (s >> 21);
                s ^= N2;
                if (i < 31)
                {
                    N2 = N1;
                    N1 = s;
                }
                else
                {
                    N2 = s;
                }
            }

            byte[] output = new byte[8];
            byte[] N1buff = BitConverter.GetBytes(N1);
            byte[] N2buff = BitConverter.GetBytes(N2);

            for (int i = 0; i < 4; i++)
            {
                output[i] = N1buff[i];
                output[4 + i] = N2buff[i];
            }

            return output;
        }
    }
}