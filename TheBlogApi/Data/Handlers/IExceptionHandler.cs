using System;
using System.Threading.Tasks;

namespace TheBlogApi.Data.Handlers
{
    public interface IExceptionHandler
    {
        Task NotifyExceptionAsync<T>(T exception, string message = null, string source = null, object data = null) where T : Exception;
        Task ProcessExceptionAsync<T>(string message, T exception, string source = null) where T : Exception;
        Task ProcessExceptionAsync<T>(string message, T exception, string source, object data) where T : Exception;
        Task ProcessExceptionAsync<T>(T exception, string source = null) where T : Exception;
    }
}