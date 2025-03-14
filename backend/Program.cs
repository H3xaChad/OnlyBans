using Microsoft.OpenApi.Models;
using OnlyBans.Backend.Config;
using OnlyBans.Backend.Extensions;
using OnlyBans.Backend.Services;
using OnlyBans.Backend.Spine.Rules;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();
builder.Configuration.AddUserSecrets<Program>();

// Console.WriteLine("===== LOADED CONFIGURATION =====");
// foreach (var kvp in builder.Configuration.AsEnumerable()) {
//     Console.WriteLine($"{kvp.Key} = {kvp.Value}");
// }
// Console.WriteLine("================================");

builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddApplicationIdentity();
builder.Services.AddOAuth2Authentication(builder.Configuration);
builder.Services.AddHttpClient<IImageService, ImageService>();
builder.Services.AddScoped<CommentService>();

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers();
    // .AddJsonOptions(options => {
    //     options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    //     options.JsonSerializerOptions.WriteIndented = true;
    // });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<RuleHandler>();
builder.Services.AddSwaggerGen(opt => {
    opt.SwaggerDoc("v1",
        new OpenApiInfo {
            Title = "OnlyBans",
            Version = "v1",
            Description = "API Description for OnlyBans"
        });
    opt.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    opt.MapType<DateOnly>(() => new OpenApiSchema {
        Type = "string",
        Format = "date"
    });
    opt.SchemaFilter<RegisterModelsFilter>();
});
builder.Services.AddCors(o => o.AddPolicy("DevCors", b => {
    b.WithOrigins(["http://127.0.0.1:5173", "http://localhost:5173"])
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
}));

var app = builder.Build();
app.ConfigureMiddleware();
app.Run();