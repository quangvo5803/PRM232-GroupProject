namespace Utilities.Email.Interface
{
    public interface IEmailQueue
    {
        void QueueEmail(string email, string subject, string htmlMessage);
        bool TryDequeue(out EmailData? emailData);
    }
}
