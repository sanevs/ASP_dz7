using Microsoft.Extensions.Options;

namespace ModelEmail;

public class SMTPUserData
{
    public string Sender { get;set; }
    public string Password { get; set;}
    public string SmtpServer { get; set;}
    public int Port { get; set;}
    public string Recipient { get; set;}
}