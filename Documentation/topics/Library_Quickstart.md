# Quickstart

<link-summary>Get up and running quickly with a default configuration of this library.</link-summary>
<card-summary>Guide to get this library set-up and running in a default configuration.</card-summary>
<web-summary>Quickstart guide to setting up and running ABSolutions.ImageToBase64 .NET library.</web-summary>

This quickstart guides you through:

- <a href="#installation" summary="Installing the NuGet package">Installation</a>
- <a href="#configuration" summary="Configuration options">Configuration</a>
- <a href="#dependency-injection" summary="Use this library via dependency injection">Dependency injection</a>
- <a href="#calling-the-library" summary="How to call this library in your code">Calling the library</a>
- <a href="#more-information" summary="Get more in-depth information about this library">More information</a>

> This quickstart is meant to provide an overview of the library and its features. It is not a comprehensive guide. More
> information is provided on separate pages of this documentation and in the demo library.

> This library has only been tested using .NET Core 8.
> {style="warning"}

## Installation

You can install this library from NuGet using your preferred method. The library is named `%ProjectLibraryName%`.
Alternatively, you can download the source code from the [GitHub repository](%GitRepo%) and incorporate the
`%ProjectLibraryName%` class-library project into your solution.

## Configuration

<include from="Shared_Snippets.topic" element-id="LibraryConfigTableWithNotes"/>

## Dependency injection

<include from="Shared_Snippets.topic" element-id="LibraryDI"/>

## Calling the library

The library only has one public method: `GetSvgAssetAsync`. This method takes a `string` parameter that represents the
filename or relative path from the `UpstreamSvgAssetBaseUri` to the SVG asset you want to retrieve. The method returns a
`Task<SvgResult>`.

To use the library, inject `ISvgAssetCollector` into your class and call the method. Here is a trivial example:

```c#
using ABSolutions.SvgAssetCollector.Services;

public class MyClass
{
    private readonly ISvgAssetCollector _svgAssetCollector;

    public MyClass(ISvgAssetCollector svgAssetCollector)
    {
        _svgAssetCollector = svgAssetCollector;
    }

    public async Task<SvgResult> DoStuffAsync()
    {
        return await _svgAssetCollector.GetSvgAssetAsync("image.svg");
    }
}
```

## Return result

<include from="Shared_Snippets.topic" element-id="returnResultStruct"/>

The `Markup` property will **never** be null. If the operation fails, the `Markup` property will contain
a [default SVG](Library_Calling-the-library.md#default-return).

## More information

If you need more information about available options or features, please refer to the other pages in this documentation.
Please also check out the `%ProjectName%.Demo.API` in the [github repo](%GitRepo%) for an example of using this library
in a minimal API. The [](Library_Build-a-demo-api.md) pages provide a step-by-step guide to building the afore mentioned
demo library.