# SvgAssetCollector: Class Library

This service reads an SVG file either from the local file system or a remote URL using an injected `HttpClient`. The SVG
file is then parsed, the main node is extracted and additional attributes are added as configured. If there are any
issues during retrieval or parsing, a default SVG is returned.

This version uses an **in-memory** cache to reduce the calls to the upstream source. The cache can be disabled as
outlined in the configuration documentation.

## Tracing

The library emits `System.Diagnostics.Activity` spans from the
`ABSolutions.SvgAssetCollector` activity source. To collect them with OpenTelemetry,
register the source in your application's tracing configuration:

```csharp
services.AddOpenTelemetry().WithTracing(tracing =>
    tracing.AddSource("ABSolutions.SvgAssetCollector"));
```

Configure an exporter separately in the host application. Spans include descriptive
operation names and may include the requested filename, cache hit and default-SVG
indicators, and attribute count. Filenames can contain sensitive data; configure your
telemetry processing accordingly. The library does not emit metrics.

**Please refer to the documentation site below for more information**

|               |                                                                     |
|---------------|---------------------------------------------------------------------|
| Git repo      | https://github.com/asifbacchus/ABSolutions.SvgAssetCollector        |
| Author        | A-B Solutions (Asif Bacchus)                                        |
| Contact       | asif@a-b.solutions                                                  |
| Documentation | https://a-b.solutions/documentation/nuget/SvgAssetCollector/Library |
| Nuget         | https://www.nuget.org/packages/ABSolutions.SvgAssetCollector        |
