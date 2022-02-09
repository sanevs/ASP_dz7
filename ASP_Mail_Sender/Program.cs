using ModelEmail;

var builder = WebApplication.CreateBuilder(args);

var configSection = builder.Configuration.GetSection("SMTPUserData");
builder.Services.Configure<SMTPUserData>(configSection);

builder.Services.AddScoped<IEmail, SMTPEmailSender>();
//builder.Services.AddHostedService<EmailBGService>();

var app = builder.Build();

CancellationToken token = new CancellationToken();

app.MapGet("/", () => 
    app.Services.CreateScope().ServiceProvider.GetRequiredService<IEmail>()
        .Send($"Ordinary send {new CurrentDateTime(DateTime.Now)}", token));

app.Run();