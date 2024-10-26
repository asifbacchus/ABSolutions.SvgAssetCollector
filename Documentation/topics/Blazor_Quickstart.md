# Quickstart

<link-summary>Get up and running quickly with a default configuration of this Blazor component.</link-summary>
<card-summary>Guide to quickly an easily start using this Blazor component with a default configuration.</card-summary>
<web-summary>Quickstart guide to setting up and using ABSolutions.SvgAssetCollector.BlazorComponent in your Blazor project.</web-summary>

This quickstart guides you through:

- <a href="#installation" summary="Installing the NuGet package"/>
- <a href="#base-library-set-up" summary="Configuring the base library"/>
- <a href="#using-this-component" summary="Referencing and calling this component"/>
- <a href="#more-information" summary="Get more in-depth information about this component"/>

> This quickstart is meant to provide a brief overview of using the Blazor component. It is not a comprehensive guide.
> More information is provided on separate pages of this document and in the demo project.

> This quickstart assumes you have a basic understanding of Blazor and how to set up a Blazor project. If you are new to
> Blazor, please consult the official Microsoft documentation and/or other educational resources.
> {style="note"}

> This component has only been tested with Blazor version 8.
> {style="warning"}

## Installation

You can install this component from NuGet using your preferred method. The package is named `%ProjectBlazorName%`. Since
it is dependent on the base library (`%ProjectLibraryName%`), that package will also be installed automatically.
Alternatively, you can download the source code from the [GitHub repository](%GitRepo%) and incorporate it into your
project manually.

## Base library set-up

This component is an implementation of the base library. As such, all the base library set-up steps apply. For
convenience, the following sections provide a non-exhaustive overview of the base library set-up. Please refer to
the [base library documentation](%DocUrl_Library%) for more detailed information.

### Configuration

<include from="Shared_Snippets.topic" element-id="LibraryConfigTableWithNotes"/>

### Dependency injection

<include from="Shared_Snippets.topic" element-id="LibraryDI"/>

## Using this component

This component is used like any other Blazor component. After it is referenced, you simply call it using the
`<SvgAssetCollector>` tag along with any desired attributes (the most obvious being the `Filename` attribute).

### Referencing the component

<tabs group="blazor_referencing">
    <tab id="blazor_referencing_perUse" title="Per use">
        To reference the component in a specific page, add the following line to the top of the <code>.razor</code> file:
        <br/>
        <code-block lang="c#">
            @using %ProjectBlazorName%
        </code-block>
    </tab>
    <tab id="blazor_referencing_global" title="Globally">
        To reference the component globally, add the following line to your <code>_Imports.razor</code> file:
        <br/>
        <code-block lang="c#">
            @using %ProjectBlazorName%
        </code-block>
    </tab>
</tabs>

### Calling the component

To call the component, use the `<SvgAssetCollector>` tag. The most basic usage would be something as follows:

```html
@page "/my-page-with-an-svg"
@using ABSolutions.SvgAssetCollector.BlazorComponent

<h1>My Page with an SVG</h1>
<p>The SVG below is injected as <em>code</em> and can be styled using CSS!</p>
<SvgAssetCollector Filename="myFile.svg"/>
```

The above example references the component directly and then uses it to render the SVG node directly in the DOM. The
`FileName` attribute specifies the name of the file to display. As explained in the base library documentation and the
configuration section above, this file name is relative to the `UpstreamSvgAssetBaseUri`. The base library will retrieve
this file from either the local system or a remote host, parse it, add any requested attributes, and return the SVG node
in a format ready for markup rendering.

Please refer to the [](Blazor_Attributes.topic) page for more information about the many attributes you can configure
and how they affect the component's behavior and the resulting `<svg>` node.

### Error handling

This Blazor component will return the [default image](Blazor_Default-image.md) if the backend library encounters *any*
errors. If you want to handle errors in a different way, I would suggest implementing the backend library manually in
your code and not using this component.

## More information

If you need more information about available options or features, please refer to the other pages in this documentation.
Please also check out the `%ProjectName%.Demo.Blazor` demo project in the [git repo](%GitRepo%) for an example of
integrating this component on its own page in the default Blazor demo project/site. If you'd prefer a step-by-step
walkthrough of constructing the demo project, please refer to the [](Blazor_Build-a-demo-blazor-site.md) pages.
