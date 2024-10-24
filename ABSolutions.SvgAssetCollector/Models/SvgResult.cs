using Microsoft.AspNetCore.Components;

namespace ABSolutions.SvgAssetCollector.Models;

/// <summary>
///     SVG Asset Collector result object with a boolean success flag and SVG data as MarkupString.
///     Default values are set to false and a currentColor exclamation mark surrounded by a circle.
/// </summary>
public struct SvgResult()
{
    public const string DefaultSvg =
        "<svg xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 24 24\"><path d=\"M12 2C6.486 2 2 6.486 2 12s4.486 10 10 10 10-4.486 10-10S17.514 2 12 2zm0 18c-4.411 0-8-3.589-8-8s3.589-8 8-8 8 3.589 8 8-3.589 8-8 8zm.5-12.5h-1v5h1v-5zm0 6h-1v1h1v-1z\" fill=\"currentColor\"/></svg>";

    /// <summary>
    ///     Whether the retrieval of the SVG data was successful.
    /// </summary>
    public bool IsSuccess { get; init; } = false;

    /// <summary>
    ///     SVG data as a MarkupString.
    /// </summary>
    public MarkupString Markup { get; init; } = new(DefaultSvg);

    /// <summary>
    ///     Create an SvgResult object with a success flag and SVG data as MarkupString.
    /// </summary>
    /// <param name="svgData">SVG markup data.</param>
    public SvgResult(string svgData) : this()
    {
        IsSuccess = true;
        Markup = new MarkupString(svgData);
    }

    /// <summary>
    ///     Create an SvgResult object with a success flag and SVG data as MarkupString.
    /// </summary>
    /// <param name="svgData">SVG markup data.</param>
    public SvgResult(MarkupString svgData) : this()
    {
        IsSuccess = true;
        Markup = svgData;
    }

    /// <summary>
    ///     Create an SvgResult object with a success/failure flag and optional SVG data as MarkupString.
    /// </summary>
    /// <param name="isSuccess">Whether the SVG retrieval process was successful.</param>
    /// <param name="svgData">SVG markup data. If omitted or null, the default SVG will be used.</param>
    public SvgResult(bool isSuccess, string? svgData = null) : this()
    {
        IsSuccess = isSuccess;
        Markup = new MarkupString(svgData ?? DefaultSvg);
    }

    /// <summary>
    ///     Create an SvgResult object with a success/failure flag and optional SVG data as MarkupString.
    /// </summary>
    /// <param name="isSuccess">Whether the SVG retrieval process was successful.</param>
    /// <param name="svgData">SVG markup data. If omitted or null, the default SVG will be used.</param>
    public SvgResult(bool isSuccess, MarkupString? svgData = null) : this()
    {
        IsSuccess = isSuccess;
        Markup = svgData ?? new MarkupString(DefaultSvg);
    }
}