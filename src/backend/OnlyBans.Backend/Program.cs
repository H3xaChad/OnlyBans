using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlyBans.Backend.Config;
using OnlyBans.Backend.Data;
using OnlyBans.Backend.Models.Users;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

var postgresLocalConfig = new PostgresConfig();
config.GetSection("PostgresLocal").Bind(postgresLocalConfig);
postgresLocalConfig.OverrideWithEnv("POSTGRES");

var postgresRemoteConfig = new PostgresConfig();
config.GetSection("PostgresRemote").Bind(postgresRemoteConfig);
postgresRemoteConfig.OverrideWithEnv("POSTGRES");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(postgresLocalConfig.GetConnectionString()));

builder.Services.AddIdentity<User, IdentityRole<Guid>>(options => {
    options.SignIn.RequireConfirmedAccount = false;
    options.User.RequireUniqueEmail = true;
    options.User.AllowedUserNameCharacters = "@abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_-.";
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireDigit = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders()
.AddRoles<UserRole>()
.AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<User, UserRole>>()
.AddDefaultTokenProviders()
.AddSignInManager();

builder.Services.AddScoped<IRoleStore<UserRole>, RoleStore<UserRole, AppDbContext, Guid>>();
builder.Services.AddScoped<IUserStore<User>, UserStore<User, UserRole, AppDbContext, Guid>>();

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();