using System;
using System.Threading.Tasks;
using TheBlogApi.Data.Context.Contracts;
using TheBlogApi.Data.Messages;

namespace TheBlogApi.Data.Context.Providers.Contracts
{
    public interface IDbContextProvider
    {
        IDbContext Context { get; }

        Task<IServiceResponse> ExecuteTransactionAsync<T>(Func<IDbContext, Task<T>> contextFunc) where T : IServiceResponse;
    }
}