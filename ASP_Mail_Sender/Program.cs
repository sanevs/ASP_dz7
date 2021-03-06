using Microsoft.Extensions.Logging.Configuration;
using ModelEmail;
using Serilog;
using Serilog.Core;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

var configSection = builder.Configuration.GetSection("SMTPUserData");
builder.Services.Configure<SMTPUserData>(configSection);

builder.Services.AddSingleton<IEmail, SMTPEmailSender>();
builder.Services.AddHostedService<EmailBGService>();
builder.Host.UseSerilog((context, config) =>
{
    config
        .WriteTo.Console(LogEventLevel.Information)
        .WriteTo.File("log-.txt", 
            LogEventLevel.Debug, 
            rollingInterval: RollingInterval.Minute);
});
var app = builder.Build();
//app.UseSerilogRequestLogging();
Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();

/*
app.MapGet("/", () => 
    app.Services.CreateScope().ServiceProvider.GetRequiredService<IEmail>()
        .Send($"Ordinary send {new CurrentDateTime(DateTime.Now).DateTime}", token));
*/
async Task<string>? Send(HttpContext context)
{
    IEmail? emailService = app.Services.GetService<IEmail>();
    try
    {
        Log.Information("{@ConnectionInfo}", context.Connection);
        return await emailService
            .SendAsync($"Ordinary send {new CurrentDateTime(DateTime.Now).DateTime}",
                default);
    }
    catch (Exception e)
    {
        string errorMessage = "Error occured while sending";
        Log.Warning(errorMessage + "{@ConnectionInfo}", context.Connection);

        try
        {
            await Task.Delay(TimeSpan.FromSeconds(10));
            return await emailService
                .SendAsync($"Second send after warning {new CurrentDateTime(DateTime.Now).DateTime}",
                    default);
        }
        catch (Exception ex)
        {
            Log.Error(e, errorMessage + "{@ConnectionInfo}", context.Connection);
            return errorMessage;
        }
    }

}

app.MapGet("/", (HttpContext context) => Send(context)?.Result);

Log.Information("Hello, server is working!");
app.Run();
