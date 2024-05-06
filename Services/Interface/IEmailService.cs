using DTO;

namespace Services.Interface
{
    public interface IEmailService
    {
        void SendEmail(EmailDTO email);
    }
}
