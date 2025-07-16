using System.Collections.Concurrent;
using Utilities.Email.Interface;

namespace Utilities.Email
{
    public class EmailQueue : IEmailQueue
    {
        private readonly ConcurrentQueue<EmailData> _queue = new();

        public void QueueEmail(string email, string subject, string htmlMessage)
        {
            _queue.Enqueue(
                new EmailData
                {
                    Email = email,
                    Subject = subject,
                    HtmlMessage = htmlMessage,
                }
            );
        }

        public bool TryDequeue(out EmailData? emailData) => _queue.TryDequeue(out emailData);
    }
}
