namespace ABSolutions.SvgAssetCollector.Models;

/// <summary>
///     SVG Asset Collector configuration options.
/// </summary>
public record SvgAssetCollectorConfiguration
{
    /// <summary>
    ///     JSON key for configuration options.
    /// </summary>
    public static readonly string AppSettingsKey = "SvgAssetCollector";

    /// <summary>
    ///     Named HTTP client to use/create for the SVG Asset Collector. Default: SvgAssetCollectorClient.
    /// </summary>
    public string HttpClientName { get; init; } = "SvgAssetCollectorClient";

    /// <summary>
    ///     Upstream SVG asset base URI. Default: http://localhost.
    /// </summary>
    public string UpstreamSvgAssetBaseUri { get; init; } = "http://localhost";

    /// <summary>
    ///     Timeout in seconds for upstream retrieval. Default: 5.
    /// </summary>
    public int UpstreamRetrievalTimeoutSeconds { get; init; } = 5;

    /// <summary>
    ///     Whether to enable SVG asset caching. Default: true.
    /// </summary>
    public bool EnableSvgCache { get; init; } = true;

    /// <summary>
    ///     Whether to set cached entries to never expire. Default: false.
    /// </summary>
    public bool NoExpiry { get; init; }

    /// <summary>
    ///     Time in minutes before SVG cache entries expire. Default: 1440 (24 hours).
    /// </summary>
    public int SvgCacheExpiryMinutes { get; init; } = 1440;

    /// <summary>
    ///     Add a key with this name to all log entries to facilitate log correlation. If empty, no key will be added. Default:
    ///     empty.
    /// </summary>
    public string LoggingCorrelationIdentifier { get; init; } = string.Empty;
}