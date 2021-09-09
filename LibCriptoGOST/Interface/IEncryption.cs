namespace LibCriptoGOST.Interface
{
    public interface IEncryption
    {
        string Encrypt(string data, string key, bool isParallel = false);
    }
}