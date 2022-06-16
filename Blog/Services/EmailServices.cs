using System.Net;
using System.Net.Mail;

namespace Blog.Services
{
    public class EmailServices
    {
        public bool Send(string toName, string toEmail,string subject, string body, string fromName = "joao", string fromEmail = "joaoassisgabriel@gmail.com")
        {
            var stmpClient = new SmtpClient(Configuration.Smtp.Host, Configuration.Smtp.Port);

            stmpClient.Credentials = new NetworkCredential(Configuration.Smtp.UserName, Configuration.Smtp.Password);
            stmpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            stmpClient.EnableSsl = true;

            var mail = new MailMessage();


            mail.From = new MailAddress(fromEmail, fromName);
            mail.To.Add(new MailAddress(toEmail,toName));
            mail.Subject = subject; 
            mail.Body = body;
            mail.IsBodyHtml = true;

            try
            {
                stmpClient.Send(mail);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
