# Calling the library

<link-summary>How to call this library to retrieve SVG assets.</link-summary>
<card-summary>Discussion of how to call this library to retrieve SVG assets.</card-summary>
<web-summary>How to call the ABSolutions.SvgAssetCollector .NET library to retrieve SVG assets.</web-summary>
<show-structure depth="2"/>

## Injecting the library

Since the library is registered with dependency injection, you can inject it anywhere you need it.

```c#
public class MyClass
{
    private readonly ISvgAssetCollector _svgAssetCollector;

    public MyClass(ISvgAssetCollector svgAssetCollector)
    {
        _svgAssetCollector = svgAssetCollector;
    }
}
```

## The GetSvgAssetAsync method

The service only has one method, `GetSvgAssetAsync`, with the following signature:

<code-block lang="c#" src="Library_libInterface.cs" include-lines="29-31"/>

| Parameter               | Explanation                                                                                                                                                                                                                               | Default                  |
|-------------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|--------------------------|
| filename                | The file (or relative path and file name) to retrieve.                                                                                                                                                                                    | `none`                   |
| attributes              | Dictionary of SVG attributes to add to the extracted SVG node.                                                                                                                                                                            | `null`                   |
| useCache                | Whether to use the in-memory cache for this particular request. If specified (not null), this overrides the global configuration setting.                                                                                                 | `null`                   |
| noExpiry                | Set the cache entry for this request to never expire. If specified (not null), this overrides the global configuration setting.                                                                                                           | `null`                   |
| loggingCorrelationValue | The value to use for log correlation. This will be the 'value' assigned to the 'key' as defined by the `LoggingCorrelationIdentifier` in `appsettings.json`. If this is an empty string, no correlation key will be included in the logs. | `empty string`           |
| cancellationToken       | Optional cancellation token to use. If none supplied, this task will not be cancellable.                                                                                                                                                  | `CancellationToken.None` |

This is an *asynchronous* method that returns a `Task<SvgResult>`. More information about
the [SvgResult](#svgresult-return-type) return type is provided later on this page.

Continuing with the previous example code where we injected the library, this is an example of how to call this method:

```c#
public class MyClass
{
    private readonly ISvgAssetCollector _svgAssetCollector;

    public MyClass(ISvgAssetCollector svgAssetCollector)
    {
        _svgAssetCollector = svgAssetCollector;
    }
    
    public async Task PrintSvgToConsole(string filename)
    {
        Console.WriteLine(await _svgAssetCollector.GetSvgAssetAsync(filename));
    }
}
```

### Default return

<link-summary>Information about the default return image.</link-summary>
If there are any problems retrieving the image or if any exceptions are thrown for whatever reason, the method will
return a default image. This allows your webpages, applications, etc. to retain their layout while still being obvious
that something has gone wrong. The default image looks like this:

![Default return image](defaultSvgReturnImage.svg){height="200" width="200"}

The image is an SVG with base-dimensions of 24x24 (pixels) and set to use the `currentColor` for its path fill. The
image is an exclamation mark surrounded by the outline of a circle. The SVG has a transparent background.

### Correlation logging

If you are using the `loggingCorrelationValue` parameter, you will need to set up the `LoggingCorrelationIdentifier` in
`appsettings.json`. Assuming both these fields are populated, the log context will be updated accordingly. For example,
let's take the following scenario:

**appsettings.json**

```json
{
  "SvgAssetCollector": {
    "LoggingCorrelationIdentifier": "TransactionId"
  }
}
```

Now, assume we call the library as follows:

```c#
Console.WriteLine(
    await _svgAssetCollector
      .GetSvgAssetAsync(filename, loggingCorrelationValue: "12345"));
```

An example JSON structured logging message would be output as follows:

```
{
  "Timestamp": "[16:33:56] ",
  "EventId": 0,
  "LogLevel": "Debug",
  "Category": "ABSolutions.SvgAssetCollector.Services.SvgAssetCollector",
  "Message": "Returning extracted SVG node from IAmRed.svg with updated attributes",
  "State": {
    "Message": "Returning extracted SVG node from IAmRed.svg with updated attributes",
    "Filename": "IAmRed.svg",
    "{OriginalFormat}": "Returning extracted SVG node from {Filename} with updated attributes"
  },
  "Scopes": [
    {
      "Message": "SpanId:afd839f83dfca818, TraceId:e755ee527b4988b94ede89f81eef2e59, ParentId:0000000000000000",
      "SpanId": "afd839f83dfca818",
      "TraceId": "e755ee527b4988b94ede89f81eef2e59",
      "ParentId": "0000000000000000"
    },
    {
      "Message": "ConnectionId:0HN7L8KEC349F",
      "ConnectionId": "0HN7L8KEC349F"
    },
    {
      "Message": "RequestPath:/svg/red RequestId:0HN7L8KEC349F:00000001",
      "RequestId": "0HN7L8KEC349F:00000001",
      "RequestPath": "/svg/red"
    },
    {
      "Message": "System.Linq.Enumerable\u002BConcat2Iterator\u00601[System.Collections.Generic.KeyValuePair\u00602[System.String,System.Object]]",
      "MethodName": "GetSvgAssetAsync",
      "TransactionId": "12345",
      "ClassName": "SvgAssetCollector"
    }
  ]
}
```

Notice our `TransactionId` is now included in the log context within the relevant scopes.

> This assumes you have set-up structured logging and the output has been formatted as human-readable (indented) JSON.
> Please refer to the demo project for an example of how to do this.

## SvgResult Return type

<include from="Shared_Snippets.topic" element-id="returnResultStruct"/>

If the file retrieval and processing was successful, `IsSuccess` will be `true` and `Markup` will contain a render-ready
SVG node with any configured attributes. If the operation failed, `IsSuccess` will be `false` and `Markup` will contain
the *default* SVG **with** any configured attributes.


> The `Markup` property will **never** be null and will *always* contain configured attributes.
