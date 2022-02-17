using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ModelEmail;

public class SMTPEmailSender : IEmail
{
    private IOptionsMonitor<SMTPUserData> _options;
    private readonly ILogger<SMTPEmailSender> _logger;
    public SMTPEmailSender(IOptionsMonitor<SMTPUserData> options, ILogger<SMTPEmailSender> logger)
    {
        _options = options;
        _logger = logger;
        _logger.LogInformation("Server started");
    }

    public async Task<string>? SendAsync(string text, CancellationToken cancellationToken = default)
    {
        var smtpUserData = _options.CurrentValue;
        using var smtpClient = new SmtpClient(smtpUserData.SmtpServer.ToString())
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

        string sendLogText = $"Email successfully sent to {smtpUserData.Recipient}";

        _logger.LogInformation(sendLogText);
        return sendLogText;
    }
}
