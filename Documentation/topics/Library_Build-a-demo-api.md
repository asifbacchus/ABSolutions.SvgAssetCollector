# Build a demo API

<link-summary>Learn about this library by building an API that uses it.</link-summary>
<card-summary>Step-by-step walkthrough building an API that uses this library.</card-summary>
<web-summary>Build an API to learn how to use ABSolutions.SvgAssetCollector .NET library.</web-summary>

The best way to understand how this library works and how to use it is to review the
`%ProjectName%.Demo.API` project in the [git repo](%GitRepo%). The project is a minimal API that has two endpoints:

1. `GET /`: The default greeting endpoint to verify the application is running. This is not an interesting endpoint.
2. `GET /svg/{filename}`: Retrieves an upstream SVG file from my development server (remote source).

While I think the code is pretty easy to follow, I know some people understand better by building things themselves. So,
I'll walk you through the process of building the demo project in the the pages contained in this section. By the end, I
think you'll have a pretty solid idea of how to integrate this library into your own projects.

> This demo project is purely for educational purposes. It is not intended to be used in production. There is no error
> trapping, logging, security/authentication or any other production-level features.
