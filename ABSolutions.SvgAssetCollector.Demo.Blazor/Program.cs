using System.Text.Json;
using ABSolutions.SvgAssetCollector.Demo.Blazor.Components;
using ABSolutions.SvgAssetCollector.DependencyInjection;
using ABSolutions.SvgAssetCollector.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging(logBuilder => logBuilder.AddJsonConsole(opts =>
{
    opts.IncludeScopes = true;
    opts.TimestampFormat = "[HH:mm:ss] ";
    opts.JsonWriterOptions = new JsonWriterOptions
    {
        Indented = true
    };
}));

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient(
    builder.Configuration.GetRequiredSection(SvgAssetCollectorConfiguration.AppSettingsKey)
        .Get<SvgAssetCollectorConfiguration>()?.HttpClientName ?? "SvgAssetCollectorClient",
    client =>
    {
        client.DefaultRequestHeaders.Add("Accept", "image/svg+xml");
        client.DefaultRequestHeaders.UserAgent.ParseAdd("SvgAssetCollector");
    });
builder.Services.AddSvgAssetCollector(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();