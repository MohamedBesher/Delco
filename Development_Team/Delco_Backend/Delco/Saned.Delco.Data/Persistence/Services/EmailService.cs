using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Saned.Delco.Data.Persistence.Tools;

namespace Saned.Delco.Data.Persistence.Services
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return configSendGridasync(message);
        }
        private Task configSendGridasync(IdentityMessage message)
        {
            EmailManager mngMail = new EmailManager();
            mngMail.SendActivationEmail(message.Subject, message.Destination, message.Body);
            return Task.FromResult(0);
        }
    }
}