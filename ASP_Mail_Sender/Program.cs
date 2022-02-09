using ModelEmail;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IEmail, SMTPEmailSender>();
builder.Services.AddHostedService<EmailBGService>();

var app = builder.Build();

CancellationToken token = new CancellationToken();
app.MapGet("/", () => 
    app.Services.CreateScope().ServiceProvider.GetRequiredService<IEmail>().Send("Ordinary send", token));

app.Run();