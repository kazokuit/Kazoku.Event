using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Website.Models.Configs;
using Website.Services;

var builder = WebApplication.CreateBuilder(args);

// Variables.
string name = "Kazoku.Event";

// Add Blazor services.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Configuration.
builder.Services.Configure<AzureOptions>(builder.Configuration.GetSection(AzureOptions.Key));

// Logging.
builder.Logging.ClearProviders();
builder.Services.AddLogging(options =>
{
    options.AddSimpleConsole(c =>
    {
        c.TimestampFormat = "[yyyy-MM-dd HH:mm:ss] ";
    });
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// Prepares HTTPS.
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

// Final setup.
app.Logger.LogInformation($"{name} is now running.");
app.Logger.LogInformation($"API Template developed by Kazoku IT AB.");
app.Logger.LogInformation($"More info can be found here: https://github.com/kazokuit/Kazoku.Template.Api.");

app.Run();
