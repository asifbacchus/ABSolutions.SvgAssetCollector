# Adding SVG attributes

<link-summary>Explanation of how the 'Attributes' library parameter works.</link-summary>
<card-summary>Understand how the 'Attributes' library parameter works using a simple example and some code.</card-summary>
<web-summary>Understand the 'Attributes' library parameter using a simple example.</web-summary>

For whatever reason, you may need to programatically add some attributes into the SVG node. Most often this is to
avoid the dreaded "flash of un-styled content" (FOUC). This happens when the SVG is first rendered at its "viewbox" size
and then resized to the size of the container as defined in your CSS. The result of this is the user seeing a brief
flash of a giant SVG and then seeing it appear the way it should have in the first place. To avoid this jarring user
experience, you can define the size of the SVG using the `height` and `width` attributes. However, this only one
instance where adding attributes can be helpful.

Let's stick with the example of adding height and width attributes. How do we do this? We may not always know the
correct size ahead of time, so we need to be able to set these attributes at render-time. This is why the library has
the `Attributes` parameter! We can see this exact situation in the demo API project in
the [SVG endpoint](Library_Demo_Create-endpoints.topic). Let's start by looking at the entire endpoint code:

<code-block src="Library_Demo_Program.cs" lang="c#" include-lines="34-54"/>

If you're not familiar with Minimal API endpoints, that's ok, we'll skip all that and just highlight the important
parts. Let's start with a dictionary that defines the attributes we want to add:

<code-block src="Library_Demo_Program.cs" lang="c#" include-lines="38-42"/>

Here you see we are defining a dictionary with the key being the attribute name and the value being the attribute value.
In this case we are setting the `height` and `width` attributes to `1234` and `4321`, respectively. Now let's look at
how we use this dictionary:

<code-block src="Library_Demo_Program.cs" lang="c#" include-lines="49-50"/>

As you can see, we pass our `sizeAttributes` dictionary as the `Attributes` parameter of our `GetSvgAssetAsync` method
call. This instructs the library to add these attributes to the retrieved SVG node or modify them if they already exist.
Thus, when the SVG node is returned, it will now contain our customized attributes!

This is just one example of how you can use the `Attributes` parameter. You can use it to add any attribute you want
including very helpful ones like `class`, `id`, `style`, `fill`, etc.


<seealso style="cards">
    <category ref="related">
        <a href="Library_Demo_Create-endpoints.topic"/>
        <a href="Library_Demo_Run-the-demo-project.md#demo-run-modifying-svg-attributes"/>
    </category>
</seealso>