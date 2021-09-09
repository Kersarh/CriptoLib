using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibCriptoGOST.Interface
{
    public interface IDecryption
    {
        public string Decrypt(string encmsg, string pass, bool isParallel = false);
    }
}
