namespace TheBlogApi.Data.Providers.Contracts
{
    public interface IHttpAccessProvider
    {
        string CurrentHost { get; }
        string CurrentProtocol { get; }
        string CurrentUrl { get; }
    }
}