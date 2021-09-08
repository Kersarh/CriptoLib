using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibCriptoAES.Interface
{
    public interface IKey
    {
        byte[] CreateKey();

        (byte[], byte[]) CreatePassword(string password);

        byte[] UsePassword(string password, byte[] salt);
    }
}
