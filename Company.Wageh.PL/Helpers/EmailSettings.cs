using System.Drawing.Drawing2D;
using System.Net;
using System.Net.Mail;

namespace Company.Wageh.PL.Helpers
{
    public class EmailSettings
    {
        public static bool SendEmail(Email email)
        {
            //Mail Server : Gmail

            //SMTP
            try 
            {
                var client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("wagehmostafadeveloper@gmail.com", "xjtatkzdpxsihnuu");
                client.Send("wagehmostafadeveloper@gmail.com", email.To, email.Subject, email.Body);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
