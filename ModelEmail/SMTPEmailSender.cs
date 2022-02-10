using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic.FileIO;

namespace ModelEmail;

public class SMTPEmailSender : IEmail
{
    private IOptionsMonitor<SMTPUserData> _options;
    public SMTPEmailSender(IOptionsMonitor<SMTPUserData> options)
    {
        _options = options;
    }
    public async Task<string> Send(string text, CancellationToken cancellationToken)
    {
        var smtpUserData = _options.CurrentValue;
        var smtpClient = new SmtpClient(smtpUserData.SmtpServer.ToString())
        {
            Port = smtpUserData.Port,
            Credentials = new NetworkCredential(smtpUserData.Sender, smtpUserData.Password),
            EnableSsl = true,
        };

        await smtpClient.SendMailAsync(
            smtpUserData.Sender,
            smtpUserData.Recipient,
            "Email test",
            text, cancellationToken);
        return $"Email successfully sent to {smtpUserData.Recipient}";
    }
}