namespace DutchTreat.Services
{
    public interface IMailService
    {
        void sendEmail(string To, string subect, string Body);
    }
}