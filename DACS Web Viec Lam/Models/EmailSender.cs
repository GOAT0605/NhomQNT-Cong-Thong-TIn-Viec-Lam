using System.Globalization;
using System.Net.Mail;
using System.Text.RegularExpressions;



namespace DACS_Web_Viec_Lam.Models
{
    public class EmailSender
    {
        public bool SendEmail(string userEmail, string link)
        {
            if (!IsValidEmail(userEmail))
            {
                // Log invalid email address
                Console.WriteLine($"Invalid email address: {userEmail}");
                return false;
            }

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("tgdd0605@gmail.com");
            mailMessage.To.Add(new MailAddress(userEmail));

            mailMessage.Subject = "Có ứng viên đã nộp đơn ứng tuyển";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = link;

            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("tgdd0605@gmail.com", "chln htzc cuvm ljqi");
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
                // Log exception
                Console.WriteLine($"Error sending email: {ex.Message}");
                return false;
            }
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper, RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    var idn = new IdnMapping();
                    string domainName = idn.GetAscii(match.Groups[2].Value);
                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }

            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}