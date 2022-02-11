namespace ModelEmail;

public class CurrentDateTime : IClock
{
    public DateTime DateTime { get; }

    public CurrentDateTime(DateTime dateTime)
    {
        DateTime = dateTime;
    }
}