# A-B Solutions: SvgAssetCollector Library and Blazor Component

This repository contains two (2) separate primary libraries:

1. `ABSolutions.SvgAssetCollector`: Retrieves an SVG file from the local file system or a remote source (via HTTP/S),
   parses the first SVG node, adds any configured attributes, and returns the SVG node ready to be rendered by a markup
   engine such as Blazor or a web-browser.
2. `ABSolutions.SvgAssetCollector.BlazorComponent`: A Blazor component that uses the `ABSolutions.SvgAssetCollector`
   library to retrieve and parse an SVG file and then insert the returned markup *inline* into the page's DOM where it
   can be easily styled using CSS. **No Javascript required!**

Usage details and release notes can be found in the README.md files of each library.

This repository also includes demo projects for each library:

- `ABSolutions.SvgAssetCollector.Demo.API`: A simple ASP.NET Core Web API demonstrating the operation of the
  `ABSolutions.SvgAssetCollector` library. The API has one endpoint that returns the first SVG node of a remote SVG file
  along with modified attributes, if so requested.
- `ABSolutions.SvgAssetCollector.Demo.Blazor`: A Blazor Server (SSR) application that uses the
  `ABSolutions.SvgAssetCollector.BlazorComponent` library to display an SVG file retrieved from a remote source and
  demonstrates the real reason these libraries were created -- placing SVGs inline and styling them using CSS **without
  Javascript!**

Each demo project is fully explained in the documentation of the respective library.
