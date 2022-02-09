namespace ModelEmail;

public interface IEmail
{
    Task<string> Send(string text, CancellationToken cancellationToken);
}