namespace LibCriptoAES.Interface
{
    public interface IDecryption
    {
        string Decrypt(string encmsg, string pass);
    }
}