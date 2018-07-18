using System.Collections.Generic;
using System.Threading.Tasks;

namespace TheBlogApi.Data.Services.Contracts
{
    public interface IEmailService
    {
        Task ConfirmationEmailNotificationAsync(string email, string token);
        Task ForgotPasswordNotificationAsync(string email, string token);
        Task SendEmailNotificationAsync(string mainAddress, string body, string title, IEnumerable<string> additionalAddesses = null);
        Task SendExceptionEmailNotificationAsync(string mainAddress, string message, string body, string title, IEnumerable<string> additionalAddesses = null);
    }
}