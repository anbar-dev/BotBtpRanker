using Application.Interfaces;
using Application.Services;
using BlazorApp.Components;
using GuerrillaNtp;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddTransient<IBondFetcher, BondFetcher>();
builder.Services.AddTransient<IBondRepository, BondRepository>();
builder.Services.AddTransient<BondService>();
builder.Services.AddTransient<NtpClock>();
builder.Services.AddTransient<NtpResponse>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

//add hosted service
//builder.Services.AddHostedService<TimedDataFetcher>();

var app = builder.Build();

// check db and tables existence
IBondRepository bondRepository = app.Services.GetRequiredService<IBondRepository>();
bondRepository.CheckDatabaseExistence();
bondRepository.CheckTablesExistence();

// initialize the periodic fetching of data


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
