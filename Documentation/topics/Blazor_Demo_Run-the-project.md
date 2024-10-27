# Run the project

<link-summary>Blazor Demo: Run the sample project with the SvgAssetCollector component.</link-summary>
<card-summary>Run the demo project to see how the SvgAssetCollector component works.</card-summary>
<web-summary>ABSolutions.SvgAssetCollector Blazor demo: Run the demo project.</web-summary>

That's it! You're ready to run this demo project.

## Main page

When you open the browser to whatever address your launch settings are configured to use, you will see the standard
Blazor demo page. Notice, however, that the menu has an entry for our *SVG* page.

## The SVG page

This page is the entire point of the demo project. When you navigate to it, you will see the page we made earlier. There
is a 50% chance you'll see the default warning exclamation mark SVGs showing that the cancellation tokens were
cancelled. If that happens, just reload the page or click on the navigation menu entry again. You may have to do this
several times (since cancellation is random, remember?) but eventually, you should see the SVGs.

When the SVGs load, you'll see there is one that announces it is blue and one that announces it is red. You will also
see one that does not exist and, thus, has been replaced with the default SVG.

Very importantly, open your browser's development tools and inspect the SVGs. Notice they are completely inlined into
the HTML markup -- there are no image or object tags! This is why the SVGs can be styled to actually be blue or red,
as appropriate. In fact, take a look at the inlined SVGs, they are not blue or red at all, they are reacting to the
CSS `currentColor`! Play around with the CSS classes and notice the results in the SVGs.
