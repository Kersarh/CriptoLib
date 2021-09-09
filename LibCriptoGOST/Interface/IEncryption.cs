using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibCriptoGOST.Interface
{
    public interface IEncryption
    {
        string Encrypt(string data, string key, bool isParallel = false);
    }
}
