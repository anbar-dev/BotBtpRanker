using Application;
using Application.Interfaces;
using BlazorApp.Components;
using BlazorApp.HostedServices;
using Infrastructure;
using Infrastructure.Options;
using System.Globalization;

// Set the default culture to Italian
var cultureInfo = new CultureInfo("it-IT");
cultureInfo.NumberFormat.CurrencySymbol = "€";

CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add sevices from different layers
builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();

// Configure options from appsettings.json
builder.Services.Configure<FetcherOptions>(
    builder.Configuration.GetSection(FetcherOptions.Key));

builder.Services.Configure<DatabaseOptions>(
    builder.Configuration.GetSection(DatabaseOptions.Key));

//add hosted service to periodically fetch new data
builder.Services.AddHostedService<TimedDataFetcher>();

var app = builder.Build();

// Check database and tables existence
IBondRepository bondRepository = app.Services.GetRequiredService<IBondRepository>();
bondRepository.CreateDatabaseIfNotExists();
bondRepository.CreateTablesIfNotExists();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();