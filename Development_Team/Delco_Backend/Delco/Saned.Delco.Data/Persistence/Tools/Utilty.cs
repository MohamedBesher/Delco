using System;
using System.Net;
using System.Net.Mail;

namespace Saned.Delco.Data.Persistence.Tools
{
    public class Utilty
    {



        public static string SendMail(
            string host,
            int port,
            string from,
            string password,
            string to,
            string subject,
            string body,
            string bbc
            )
        {
            // Configure mail client (may need additional
            // code for authenticated SMTP servers)


            SmtpClient mailClient = new SmtpClient(host)
            {
                EnableSsl = false,
                Port = port,
                Credentials = new System.Net.NetworkCredential(@from, password),
                UseDefaultCredentials = false
            };

            MailMessage mailMessage = new MailMessage(from, to, subject, body) { IsBodyHtml = true };

            try
            {
                mailClient.Send(mailMessage);
                return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;
                // throw ex;
            }

        }


        public static void sendFromGmail(string to,string subject, string body)
        {
            var fromAddress = new MailAddress("saned.inc@gmail.com", "saned.inc@gmail.com");
            var toAddress = new MailAddress(to, to);
            const string fromPassword = "2.Asaned";
        
     

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }





    }
}