using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibCriptoAES.Interface
{
    public interface IDecryption
    {
        string Decrypt(string encmsg, string pass);
    }
}
