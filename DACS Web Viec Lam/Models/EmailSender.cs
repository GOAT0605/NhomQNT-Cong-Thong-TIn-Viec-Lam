using System.Globalization;
using System.Net.Mail;
using System.Text.RegularExpressions;



namespace DACS_Web_Viec_Lam.Models
{
    public class EmailSender
    {
        public bool SendEmail(string userEmail, string link)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("galaxye7pro9@gmail.com");
            mailMessage.To.Add(new MailAddress(userEmail));

            mailMessage.Subject = "Cảm ơn bạn đã đóng góp";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = link;

            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("galaxye7pro9@gmail.com", "ilej muev fwom daoy");
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;

            try
            {
                client.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                // log exception
            }
            return false;
        }
    }
}