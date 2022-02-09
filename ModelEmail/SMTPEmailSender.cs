using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic.FileIO;

namespace ModelEmail;

public class SMTPEmailSender : IEmail
{
    private SMTPUserData _sMTPUserData;
    public SMTPEmailSender(IOptions<SMTPUserData> options)
    {
        _sMTPUserData = options.Value;
    }
    public async Task<string> Send(string text, CancellationToken cancellationToken)
    {
        var smtpClient = new SmtpClient(_sMTPUserData.SmtpServer.ToString())
        {
            Port = _sMTPUserData.Port,
            Credentials = new NetworkCredential(_sMTPUserData.Sender, _sMTPUserData.Password),
            EnableSsl = true,
        };

        await smtpClient.SendMailAsync(
            _sMTPUserData.Sender,
            _sMTPUserData.Recipient,
            "Email test",
            text, cancellationToken);
        return "Success";
    }
}