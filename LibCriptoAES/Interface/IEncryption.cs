namespace LibCriptoAES.Interface
{
    public interface IEncryption
    {
        string Encrypt(string message, string pass);
    }
}