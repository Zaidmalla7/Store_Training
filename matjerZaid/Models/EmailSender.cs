using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Net;


namespace ECApp.Model
{
    public class EmailSender : IEmailSender
    {
       
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var femal = "zaidalmallah444@gmail.com";
            var fpassword = "scoj ffxo aquu idyr";


            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(femal);
            mailMessage.Subject = subject;
            mailMessage.To.Add(email);
            mailMessage.Body = htmlMessage;
            mailMessage.IsBodyHtml = true;

            var smtpClint = new SmtpClient("smtp.gmail.com")
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(femal,fpassword),
                Port = 587
            };
         
            smtpClint.Send(mailMessage);
           

        }

    }
}
