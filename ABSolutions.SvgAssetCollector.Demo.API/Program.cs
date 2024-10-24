using System.Text.Json;
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
    })
);

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

app.MapGet("/", () => "Hello World!");

app.Run();