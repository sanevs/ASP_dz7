using Microsoft.Extensions.Hosting;

namespace ModelEmail;

public class EmailBGService : BackgroundService
{
    private readonly IEmail _sender;

    public EmailBGService(IEmail sender)
    {
        _sender = sender;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var timer = new PeriodicTimer(TimeSpan.FromSeconds(30));
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            _sender.Send(
                $"Сервер работает, используемая память {GC.GetTotalMemory(false)} байтов", 
                stoppingToken);
        }
    }
}