using HistoricalMonumentsWebApplication.Models;

namespace HistoricalMonumentsWebApplication.Services
{
    public interface IEmailSender
    {
        void SendEmail(Message message);
    }
}
