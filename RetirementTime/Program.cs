using RetirementTime.Components;
using RetirementTime.Application;
using RetirementTime.Infrastructure;
using RetirementTime.Services;
using System.Globalization;
using Microsoft.AspNetCore.Localization;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
    
// Add localization services
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserLanguageCultureProvider>();


var app = builder.Build();

// Configure supported cultures
var supportedCultures = new[] 
{ 
    new CultureInfo("en"), 
    new CultureInfo("fr") 
};

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures,
    RequestCultureProviders = new List<IRequestCultureProvider>
    {
        new UserLanguageCultureProvider()
    }
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();