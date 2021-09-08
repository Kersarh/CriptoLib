using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibCriptoAES.Interface
{
    public interface IEncryption
    {
        string Encrypt(string message, string pass);
    }
}
