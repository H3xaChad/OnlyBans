using OnlyBans.Backend.Extensions;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();
builder.Configuration.AddUserSecrets<Program>();

Console.WriteLine("===== LOADED CONFIGURATION =====");
foreach (var kvp in builder.Configuration.AsEnumerable()) {
    Console.WriteLine($"{kvp.Key} = {kvp.Value}");
}
Console.WriteLine("================================");

builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddApplicationIdentity();
builder.Services.AddOAuth2Authentication(builder.Configuration);

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.ConfigureMiddleware();
app.Run();