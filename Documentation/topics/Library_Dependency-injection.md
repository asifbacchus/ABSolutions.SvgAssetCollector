# Dependency injection

<link-summary>Explanation of how dependency injection is used by this library.</link-summary>
<card-summary>Explanation of how dependency injection is used by this library.</card-summary>
<web-summary>Explanation of dependency injection usage in ABSolutions.SvgAssetCollector .NET library.</web-summary>

This library is best used via dependency injection. To make this easier, this library includes a `ServiceCollection`
extension method that will register the in-memory cache and collector services for you. The extension method requires a
reference to the configuration container to read settings from `appsettings.json`.

While the extension method registers all services specific to this library, you **MUST** still register a named
`HttpClient` provider for the library to use. The named instance **MUST** match the name of the `HttpClient`
configuration
property the `appsettings.json` file.

> If the name of the `HttpClient` does not match the one specified in the library configuration (`appsettings.json`), a
> default `HttpClient` instance will be used instead of your customized one!
> {style="warning"}

Here's an example, using default values, of how you can handle all necessary dependency injection in your `program.cs`.
This can be placed anywhere in the build section (before `builder.Build()`):

```c#
builder.Services.AddHttpClient("SvgAssetCollectorClient");
builder.Services.AddImageToBase64(builder.Configuration);
```

<seealso style="cards">
    <category ref="related">
        <a href="Library_HTTP-client.md"/>
    </category>
</seealso>
