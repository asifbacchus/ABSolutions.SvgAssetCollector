# Default image

In the event that a component call is cancelled or fails for **any** reason, the backend `SvgAssetCollector` library
will return the following default image:

![DefaultImage](defaultSvgReturnImage.svg){height="250" width="250"}

This image is an SVG with a viewport size of 24x24 pixels. It is an exclamation mark surrounded by a circular outline,
both coloured according to the `currentColor`. Since this is an SVG, it can be scaled to any size either via the
[component attributes](Blazor_Attributes.topic) or by your stylesheet (CSS). The example on this page has been scaled to
250x250 pixels.

Returning a default image prevents layout-breaking errors in the frontend due to missing images. It also provides a
visual cue that something went wrong.

> If you'd prefer to handle retrieval errors in a different way, I'd suggest directly calling the backend library in a
> code-behind section and not using this Blazor component.
> {style="warning"}
