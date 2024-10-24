using ABSolutions.SvgAssetCollector.Models;

namespace ABSolutions.SvgAssetCollector.Services;

public interface ISvgAssetCollector
{
    /// <summary>
    ///     Retrieves an SVG asset from an upstream asset source and returns it as a markup string.
    /// </summary>
    /// <param name="filename">Filename or 'slug' of the upstream asset, optionally with a relative path.</param>
    /// <param name="attributes">List of key value pairs representing desired SVG element attributes to be added.</param>
    /// <param name="useCache">
    ///     Use cached result if available and not expired. If not specified, configuration value will be
    ///     applied.
    /// </param>
    /// <param name="noExpiry">
    ///     If a cached entry is created or refreshed due to this call, set that cached entry to NEVER
    ///     expire. If not specified, configuration value will be applied.
    /// </param>
    /// <param name="loggingCorrelationValue">
    ///     Use this value for the configured logging correlation key. If empty, correlation
    ///     key will not be added.
    /// </param>
    /// <param name="cancellationToken">Cancellation token. Optional.</param>
    /// <returns>
    ///     SvgResult object containing a success flag and a markup string representing the retrieved SVG. The markup
    ///     string will always be populated regardless of success condition (i.e. never null).
    /// </returns>
    Task<SvgResult> GetSvgAssetAsync(string? filename, List<KeyValuePair<string, string>>? attributes,
        bool? useCache = null, bool? noExpiry = null, string loggingCorrelationValue = "",
        CancellationToken cancellationToken = default);
}