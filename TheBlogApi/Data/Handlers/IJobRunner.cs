using System.Threading.Tasks;

namespace TheBlogApi.Data.Handlers
{
    public interface IJobRunner
    {
        Task RunTriggeredJobAsync();
    }
}