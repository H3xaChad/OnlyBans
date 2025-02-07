using OnlyBans.Backend.Extensions;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();
builder.Configuration.AddUserSecrets<Program>();

builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddApplicationIdentity();
// builder.Services.AddApplicationAuthentication(builder.Configuration);

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.ConfigureMiddleware();
app.Run();