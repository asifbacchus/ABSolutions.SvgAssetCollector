# Run the demo project

<link-summary>API Demo: Running the project to see how this library works.</link-summary>
<card-summary>Run the demo project to see how this library works.</card-summary>
<web-summary>ABSolutions.SvgAssetCollector API demo: Running the demo project.</web-summary>

That's it! You're ready to run this demo project. I'd recommend using your IDE's built-in HTTP Client or a tool like
*Insomnia* or *Postman* since these are much easier ways to interact with an API versus command-line tools like cURL or
Powershell. Either way, the API will return a `MarkupString`, which is just text, so you should have no problems viewing
it.

## Root endpoint

If you submit a GET request to the main (root) endpoint, you'll be greeted with the default Minimal API welcome
message: "Hello World!". This is not exciting. Let's check out the other endpoint.

## SVG endpoint

Submit a GET request to `/svg/{filename}` to retrieve an SVG file from my development server:

- `/svg/blue`: Will return an SVG as a `MarkupString` with a status 200.
- `/svg/red`: Will return an SVG as a `MarkupString` with a status 200.
- `/svg/???`: Anything other than the above options will return the *default* SVG as a `MarkupString` with status 404.

### Modifying SVG attributes {id="demo-run-modifying-svg-attributes"}

<link-summary>API Demo: Easily see the effect of applying attributes by toggling the 'addAttributes' query parameter</link-summary>
<card-summary>API Demo: Easily see the effect of applying attributes by toggling the 'addAttributes' query parameter</card-summary>
If you retrieve the SVG as above, you'll notice each SVG has `height=\"100\"` and `width=\"400\"` attributes. To see the
effect of the `Attributes` parameter, submit a GET request to `/svg/{filename}?addAttributes=true` where `filename` is
either `blue` or `red`.

Notice now that the SVG has `height=\"1234\"` and `width=\"4321\"` attributes. This is the effect of the `Attributes`
parameter applying the `sizeAttributes` dictionary values to the retrieved SVG node.

## Caching

You'll notice if you make multiple requests to the same endpoint and route parameter (i.e. `/svg/blue`), you'll get a
response *very* quickly. This is because the response is being cached. Each time you make *the same* request, the cache
will be refreshed and the expiration timer will be reset. If you wait 5 minutes and then make a request to the same
endpoint and route parameter, the library will have to reach out to the upstream server and you'll notice that request
will be slower. This is how the in-memory cache works.
