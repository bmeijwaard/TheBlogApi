namespace TheBlogApi.Data.Providers.Contracts
{
    public interface ICryptoProvider
    {
        string DecryptPrivate(string input);
        string DecryptPublic(string input);
        string EncryptPrivate(string input);
        string EncryptPublic(string input);
    }
}