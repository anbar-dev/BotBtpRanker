using Application;
using Application.Interfaces;
using BlazorApp.Components;
using BlazorApp.HostedServices;
using Infrastructure;
using Infrastructure.Options;
using System.Globalization;

//CultureInfo cultureInfo = new("it-IT", false);

var cultureInfo = new CultureInfo("it-IT");
cultureInfo.NumberFormat.CurrencySymbol = "€";

CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// add sevices from different layers
builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();

builder.Services.Configure<FetcherOptions>(
    builder.Configuration.GetSection(FetcherOptions.Key));

builder.Services.Configure<DatabaseOptions>(
    builder.Configuration.GetSection(DatabaseOptions.Key));

//add hosted service to periodically fetch new data
builder.Services.AddHostedService<TimedDataFetcher>();  //TEMPORARILY COMMENTED OUT TO AVOID CONSTANT FETCHING AT EVERY LAUNCH

var app = builder.Build();

// check db and tables existence
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