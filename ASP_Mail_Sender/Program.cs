using ModelEmail;

var builder = WebApplication.CreateBuilder(args);

var configSection = builder.Configuration.GetSection("SMTPUserData");
builder.Services.Configure<SMTPUserData>(configSection);

builder.Services.AddSingleton<IEmail, SMTPEmailSender>();//(new SMTPEmailSender(configSection.Get<SMTPUserData>()));
builder.Services.AddHostedService<EmailBGService>();

var app = builder.Build();

CancellationToken token = new CancellationToken();
IClock dateTime = new CurrentDateTime(DateTime.Now);
app.MapGet("/", () => 
    app.Services.CreateScope().ServiceProvider.GetRequiredService<IEmail>()
        .Send($"Ordinary send {dateTime.DateTime}", token));

app.Run();