namespace LibCriptoGOST.Interface
{
    public interface IDecryption
    {
        public string Decrypt(string encmsg, string pass, bool isParallel = false);
    }
}