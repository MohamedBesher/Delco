using System;
using Saned.Delco.Data.Core;
using Saned.Delco.Data.Core.Models;
using System.Net;
using System.IO;

namespace Saned.Delco.Data.Persistence.Tools
{
    public class EmailManager : IDisposable
    {
        private readonly IUnitOfWorkAsync _unitOfWork;

        public EmailManager()
        {
            _unitOfWork = new UnitOfWorkAsync();
        }


        public string SendActivationEmail(string messageTamplate, string toEmail, string messageBodyAr)
        {
            EmailSetting emailSettings = _unitOfWork.EmailSetting.GetEmailSetting(messageTamplate);

            var result = Utilty.SendMail(emailSettings.Host,
                emailSettings.Port,
                emailSettings.FromEmail,
                emailSettings.Password,
                toEmail,
                emailSettings.SubjectAr,
                messageBodyAr, "");

            return result;

        }

        public string SendWelcomeEmail(string messageTamplate, string toEmail, string messageBodyAr)
        {
            EmailSetting emailSettings = _unitOfWork.EmailSetting.GetEmailSetting(messageTamplate);

            var result = Utilty.SendMail(emailSettings.Host,
                emailSettings.Port,
                emailSettings.FromEmail,
                emailSettings.Password,
                toEmail,
                emailSettings.SubjectAr,
                messageBodyAr, "");

            return result;

        }

        public string SendAEmail(string host, int port, string fromEmail, string password, string toEmail, string messageBodyAr, string subjectAr)
        {
            //EmailSetting emailSettings = _unitOfWork.EmailSetting.GetEmailSetting(messageTamplate);         

            var result = Utilty.SendMail(host,
                port,
                fromEmail,
                password,
                toEmail,
                subjectAr,
                messageBodyAr, "");

            return result;

        }

        public string SendActivationEmail(
            string messageTamplate,
            int port,
            string toEmail,
            string messageBodyAr,
            string host,
            string fromEmail,
            string password,
            string subjectAr
            )
        {

            var result = Utilty.SendMail(host, port, fromEmail, password, toEmail, subjectAr, messageBodyAr, "");
            return result;

        }


        public bool SendSMS(string apiRoute, string number, string message)
        {
            try
            {
                apiRoute = string.Format(apiRoute, message, number);

                var request = WebRequest.Create(apiRoute);
                request.ContentType = "application/json; charset=utf-8";
                string text;
                var response = (HttpWebResponse)request.GetResponse();

                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    text = sr.ReadToEnd();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _unitOfWork.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
