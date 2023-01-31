
using BusReservationSystemApi.Data.Configuration;

namespace BusReservationSystemApi.Services.EmailSenderService
{
    public interface IEmailSenderService
    {
        Task SendEmailAsync(Message message);
    }
}
