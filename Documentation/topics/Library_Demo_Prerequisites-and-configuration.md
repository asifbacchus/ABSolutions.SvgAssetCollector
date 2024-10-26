# Prerequisites and configuration

<link-summary>API Demo: Setting up and configuring the library for use in the API demo.</link-summary>
<card-summary>Learn how to set-up and configure the library for use in the API demo project.</card-summary>
<web-summary>ABSolutions.SvgAssetCollector API demo: Configuring the library.</web-summary>

## Project template

Create a new `Empty ASP.NET Core Web Application` project. Use .NET 8.0.
> An *empty* web application project is the typical
> template used when creating a minimal API project.

## NuGet packages

Please install the `%ProjectLibraryName%` NuGet package.

## Configuration

Let's go ahead and configure the library by modifying the `appsettings.json` file to appear as follows:

<code-block lang="json" src="Library_Demo_appsettings.json"/>

Everything is default except for the `UpstreamSvgAssetBaseUri`. We'll be using my development server to provide us with
two properly formatted SVG images. I have also specified `TransactionId` as my `LoggingCorrelationIdentifier` to
demonstrate how this library can be integrated in a correlated logging solution.

> I've changed the logging level for the `ABSolutions.SvgAssetCollector` namespace to `Debug`. This is optional, but it
> will let you see more output on the console and better understand what the library is doing.

## Logging

To demonstrate how this package outputs structured logging information, we'll use the included Microsoft Logging
Extensions but configured for somewhat nauseatingly verbose JSON output. I always prefer to configure logging before
anything else in my program, so I modified the beginning of my `Program.cs` as follows:

<code-block lang="c#" src="Library_Demo_Program.cs" include-lines="7-18"/>

If you're not interested in structured logging or don't care for the verbose output, you can modify or even omit this
entirely. If you don't configure anything, you'll still get standard text output to the console but you will not see
the 'correlation logging' option output.

## Register services

To use this library, we need to register two services with the dependency injection container: the `HttpClient` service
and the `Base64Converter` service.

### HTTP Client

We'll be using a lightly customized named `HttpClient` instance using the default name the library expects
(`SvgAssetCollectorClient`) and two custom request headers as an example:

<code-block src="Library_Demo_Program.cs" include-lines="20-27" lang="c#"/>

> The `Accept` header tells the upstream service that we only want SVG files. This is strongly recommended.
> The `UserAgent` header is included as an example of other headers that can be added.

This can be added to the `Program.cs` anywhere before the `builder.Build()` method is called.

### SVGAssetCollector service

We'll use the library's service extension method to register the library's services with the dependency injection
container. Since the extension method requires access to the service configuration in `appsettings.json`, we'll pass the
builder configuration object as an argument.

You can add this registration call anywhere in the `Program.cs` file before the `builder.Build()` method is called.
Here's an example along with the `HttpClient` registration:

<code-block src="Library_Demo_Program.cs" include-lines="20-30" lang="c#"/>
