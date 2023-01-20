using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
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

// Add services to the container.
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));
builder.Services.AddControllersWithViews()
    .AddMicrosoftIdentityUI();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<StorageService>();

builder.Services.AddAuthorization(options =>
{
    // By default, all incoming requests will be authorized according to the default policy
    options.FallbackPolicy = options.DefaultPolicy;
});

// Logging.
builder.Logging.ClearProviders();
builder.Services.AddLogging(options =>
{
    options.AddSimpleConsole(c =>
    {
        c.TimestampFormat = "[yyyy-MM-dd HH:mm:ss] ";
    });
});

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor()
    .AddMicrosoftIdentityConsentHandler();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

// Final setup.
app.Logger.LogInformation($"{name} is now running.");
app.Logger.LogInformation($"API Template developed by Kazoku IT AB.");
app.Logger.LogInformation($"More info can be found here: https://github.com/kazokuit/Kazoku.Template.Api.");


app.Run();
