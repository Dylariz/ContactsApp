using System.Reflection;
using ContactsApp.Data;
using ContactsApp.Services;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

var builder = WebApplication.CreateBuilder(args);
//builder.WebHost.UseUrls("https://192.168.1.66:7259;https://localhost:5000");

builder.Host.UseSerilog();
var services = builder.Services;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(theme: AnsiConsoleTheme.Code)
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .CreateLogger();
services.AddSingleton(Log.Logger);

// Add services to the container.
services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.ConfigureSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ContactsApp API",
        Description = "ASP.NET Core API of the ContactsApp project",
        Contact = new OpenApiContact { Name = "Github", Url = new Uri("https://github.com/dylariz") },
        License = new OpenApiLicense
            { Name = "MIT License", Url = new Uri("https://github.com/Dylariz/ContactsApp/blob/master/LICENSE") }
    });
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

services.AddScoped<IAuntificationService, AuntificationService>();
services.AddScoped<IContactsService, ContactsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(
    x =>
    {
        x.AllowAnyHeader();
        x.AllowAnyMethod();
        x.AllowAnyOrigin();
    });

app.UseHttpsRedirection();

app.MapControllers();

DBUtils.PrepareDatabase();

await app.RunAsync();