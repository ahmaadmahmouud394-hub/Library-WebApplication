using System.Net.Mail;

namespace Library_WebApplication.Services
{
    public class EmailValidation
    {
        public bool IsEmailValid(string mail)
        {
            MailAddress.TryCreate(mail, out var emailAddress);
            return emailAddress != null;
        }
        public string GetEmailDomain(string emailAddress)
        {
            try
            {
                MailAddress mailAddress = new MailAddress(emailAddress);
                return mailAddress.Host; // This returns the domain part
            }
            catch (FormatException)
            {
                // Handle invalid email format
                return null;
            }
        }
    }
}
