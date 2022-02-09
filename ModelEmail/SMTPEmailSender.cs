using System.Net;
using System.Net.Mail;

namespace ModelEmail;

public class SMTPEmailSender : IEmail
{
    private const string Sender = "asp2022@rodion-m.ru";
    
    public async Task<string> Send(string text, CancellationToken cancellationToken)
    {
        var smtpClient = new SmtpClient("smtp.beget.com")
        {
            Port = 25,
            Credentials = new NetworkCredential(Sender, "aHGnOlz7"),
            EnableSsl = true,
        };

        await smtpClient.SendMailAsync(
            Sender,
            "vavland@mail.ru",
            "Email test",
            text, cancellationToken);
        return "Success";
    }
}