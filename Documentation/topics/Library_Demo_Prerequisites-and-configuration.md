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

<include from="Shared_Snippets.topic" element-id="DemoLibraryConfiguration"/>

## Logging

<include from="Shared_Snippets.topic" element-id="DemoLibraryLoggingConfiguration"/>

## Register services

To use this library, we need to register two services with the dependency injection container: the `HttpClient` service
and the `Base64Converter` service.

### HTTP Client

<include from="Shared_Snippets.topic" element-id="DemoHttpClientConfiguration"/>

### SVGAssetCollector service

<include from="Shared_Snippets.topic" element-id="DemoLibraryServiceDI"/>
