namespace ModelEmail;

public interface IEmail
{
    Task<string>? SendAsync(string text, CancellationToken cancellationToken);
}