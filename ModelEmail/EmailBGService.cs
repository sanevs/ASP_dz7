using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;

namespace ModelEmail;

public class EmailBGService : BackgroundService
{
    private readonly IEmail _sender;
    private readonly ILogger<SMTPEmailSender> _logger;
    private const int RetryCount = 5;

    public EmailBGService(IEmail sender, ILogger<SMTPEmailSender> logger)
    {
        _sender = sender;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var policy = Policy.Handle<Exception>()
            .RetryAsync(RetryCount, (exception, i) => 
                _logger.LogWarning(exception, "Error while sending email. Retrying: {Attempt}", i));
        var timer = new PeriodicTimer(TimeSpan.FromSeconds(30));
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            var result = await policy.ExecuteAndCaptureAsync( token =>
            _sender.SendAsync(
                $"Сервер работает, используемая память {GC.GetTotalMemory(false)} байтов", 
                token), stoppingToken);
            if(result.Outcome == OutcomeType.Failure)
                _logger.LogError(result.FinalException, "There was an error while sending email");
        }
    }
}