using ABSolutions.SvgAssetCollector.Models;
using Microsoft.AspNetCore.Components;

namespace ABSolutions.SvgAssetCollector.Services;

/// <summary>
///     Get or set an SVG object in the cache.
/// </summary>
public interface ISvgCache
{
    /// <summary>
    ///     Register an SVG markup string in the cache.
    /// </summary>
    /// <param name="filename">Filename of the SVG, used as an identifier.</param>
    /// <param name="svg">SVG markup string.</param>
    /// <param name="expiryMinutes">Number of minutes until cached object expires. Set to 0 if object should NEVER expire.</param>
    /// <returns>Boolean result of registering a new cached object.</returns>
    public ValueTask<(bool result, Exception? exception)> RegisterAsync(string filename, MarkupString svg,
        int expiryMinutes);

    /// <summary>
    ///     Retrieve a cached SVG object.
    /// </summary>
    /// <param name="filename">Filename of the SVG to retrieve from the cache.</param>
    /// <returns>SVG markup string or null if nothing found in cache.</returns>
    public ValueTask<SvgCachedObject?> GetCachedSvg(string filename);
}