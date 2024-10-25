# HTTP Client

<link-summary>How to configure the HTTP client for use with this library.</link-summary>
<card-summary>Explanation and examples of how to configure the HTTP client for use with this library.</card-summary>
<web-summary>Configuring the HTTP client for use with ABSolutions.SvgAssetCollector .NET library.</web-summary>

To make this library more flexible and to account for the numerous possible upstream configuration cases, this library
expects you to separately configure the `HttpClient` it will use. This also allows you to share the client
between services, if desired.

The `HttpClient` should be configured as per the `IHttpClientFactory` pattern. This allows you to configure many options
such as default request headers, resilience policies and even delegating handlers for things like authentication.

> The most important configuration option is the `HttpClient` name. This must match the name you specify in the library
> configuration in `appsettings.json`.

## Example configurations

The following examples all use the library's default expected `HttpClient` name of *SvgAssetCollectorClient*. You may,
of course, change this to suit your application. If you do, remember to update the `HttpClientName` in
`appsettings.json` to match your chosen name.

### Basic configuration

```c#
builder.Services.AddHttpClient("SvgAssetCollectorClient");
```

This will create an `HttpClient` named "SvgAssetCollectorClient" with default settings.

### Set response headers

```c#
builder.Services.AddHttpClient("SvgAssetCollectorClient", client =>
{
    client.DefaultRequestHeaders.Add("Accept", "image/svg+xml");
    client.DefaultRequestHeaders.UserAgent.ParseAdd("MyApplication");
});
```

This will create an `HttpClient` named "SvgAssetCollectorClient".

- An `Accept: image/svg+xml` request header will be added to all requests by default. The library actually adds this
  header for you, in case you forget to configure it.
- A `User-Agent` header will be added with the value `MyApplication`. This is just an example of adding additional
  headers.

### Upstream authentication

Since methods and requirements for upstream authentication can vary greatly, this is probably the best example of why
the choice was made to require injecting a separately configured `HttpClient`. In general, something like this would be
used for *bearer token* authentication:

```c#
builder.Services.AddHttpClient("SvgAssetCollectorClient", client =>
{
    client.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer", "token123456");
});
```

> In production you will probably either retrieve the token from a secure vault or some form of secured configuration
> file. Alternatively, you can create a *delegating handler* to handle authentication as needed. The latter option is
> recommended but is beyond the scope of this document.
