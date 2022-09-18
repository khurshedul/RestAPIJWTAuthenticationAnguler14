using System.Threading.Tasks;

namespace Core.Utils.Interfaces
{

    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
