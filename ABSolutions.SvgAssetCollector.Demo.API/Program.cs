using System.Text.Json;
using ABSolutions.SvgAssetCollector.DependencyInjection;
using ABSolutions.SvgAssetCollector.Models;
using ABSolutions.SvgAssetCollector.Services;
using Microsoft.AspNetCore.Mvc;

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

app.MapGet("/svg/{filename}",
    async (string filename, [FromServices] ISvgAssetCollector svgAssetCollector) =>
    {
        var correlationValue = Guid.NewGuid().ToString();
        var requestFilename = filename switch
        {
            "blue" => "IAmBlue.svg",
            "red" => "IAmRed.svg",
            _ => "noExist.svg"
        };
        var svg = await svgAssetCollector.GetSvgAssetAsync(requestFilename, loggingCorrelationValue: correlationValue);
        return svg.IsSuccess ? Results.Ok(svg.Markup.Value) : Results.NotFound(svg.Markup.Value);
    });

app.Run();