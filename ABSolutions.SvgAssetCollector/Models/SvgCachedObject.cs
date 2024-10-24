using Microsoft.AspNetCore.Components;

namespace ABSolutions.SvgAssetCollector.Models;

/// <summary>
///     SVG cached object with optional expiry date.
/// </summary>
public record SvgCachedObject
{
    public required string Filename { get; init; }
    public required MarkupString Svg { get; init; }
    public DateTime? Expiry { get; init; }

    /// <summary>
    ///     Override ToString() to provide relevant debugging output.
    /// </summary>
    /// <returns>String representation of this object.</returns>
    public override string ToString()
    {
        return
            $"Filename: {Filename} | SVG: {(string.IsNullOrWhiteSpace(Svg.Value) ? "<null>" : "<has value>")} | Expiry: {(Expiry.HasValue ? Expiry.Value.ToString("O") : "infinite")}";
    }
}