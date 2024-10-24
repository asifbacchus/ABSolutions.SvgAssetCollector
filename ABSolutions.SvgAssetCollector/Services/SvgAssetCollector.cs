using System.Xml;
using ABSolutions.SvgAssetCollector.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ABSolutions.SvgAssetCollector.Services;

public class SvgAssetCollector : ISvgAssetCollector
{
    private readonly bool _accessLocalFiles;

    private readonly Dictionary<string, object> _baseLogContexts = new()
    {
        {"ClassName", nameof(SvgAssetCollector)}
    };

    private readonly SvgAssetCollectorConfiguration _configuration;
    private readonly string _fileBaseDirectory = "/";
    private readonly Uri _httpBaseAddress = new("http://localhost/");
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly TimeSpan _httpTimeout = TimeSpan.FromSeconds(30);
    private readonly ILogger<SvgAssetCollector>? _logger;
    private readonly ISvgCache _svgCache;

    public SvgAssetCollector(IOptions<SvgAssetCollectorConfiguration> configuration,
        IHttpClientFactory httpClientFactory, ILogger<SvgAssetCollector>? logger, ISvgCache svgCache)
    {
        _configuration = configuration.Value;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _svgCache = svgCache;

        var logContexts = new Dictionary<string, object>
        {
            {"MethodName", "ctor"}
        };
        using (_logger?.BeginScope(logContexts.Concat(_baseLogContexts)))
        {
            // construct base URI
            _logger?.LogDebug("Upstream SVG asset source configured as: {UpstreamSvgAssetBaseUri}",
                _configuration.UpstreamSvgAssetBaseUri);
            var configuredBaseAddress = _configuration.UpstreamSvgAssetBaseUri.Trim().TrimEnd('/', '\\') + "/";
            if (configuredBaseAddress.StartsWith("file://"))
            {
                _logger?.LogInformation(
                    "Upstream SVG asset source ({UpstreamSvgAssetBaseUri}) is a file URI which may not be available to clients or when running on a remote host",
                    configuredBaseAddress);
                _fileBaseDirectory =
                    Path.Combine(configuredBaseAddress.Trim().Replace("file://", "").Replace('\\', '/').Split('/'));
                if (!Directory.Exists(_fileBaseDirectory))
                {
                    _logger?.LogError("Upstream SVG asset source ({UpstreamSvgAssetBaseUri}) is not a valid directory",
                        _fileBaseDirectory);
                    throw new DirectoryNotFoundException(
                        $"Upstream SVG asset source ({configuredBaseAddress}) is not a valid directory");
                }

                _accessLocalFiles = true;
                _logger?.LogInformation("Upstream SVG asset source: {UpstreamSvgAssetBaseUri}", _fileBaseDirectory);
            }
            else
            {
                if (!Uri.IsWellFormedUriString(configuredBaseAddress, UriKind.Absolute))
                {
                    _logger?.LogError(
                        "Upstream SVG asset source ({UpstreamSvgAssetBaseUri}) is not a valid absolute URI",
                        configuredBaseAddress);
                    throw new UriFormatException(
                        $"Upstream SVG asset source ({configuredBaseAddress}) is not a valid absolute URI");
                }

                _httpBaseAddress = new Uri(configuredBaseAddress);
                _logger?.LogInformation("Upstream SVG asset source: {UpstreamSvgAssetBaseUri}", _httpBaseAddress);

                // construct timeout
                _httpTimeout = TimeSpan.FromSeconds(_configuration.UpstreamRetrievalTimeoutSeconds);
                _logger?.LogInformation("Upstream SVG retrieval timeout: {UpstreamSvgRetrievalTimeoutSeconds} seconds",
                    _httpTimeout);
            }
        }
    }

    public async Task<SvgResult> GetSvgAssetAsync(string? filename, List<KeyValuePair<string, string>>? attributes,
        bool? useCache = null, bool? noExpiry = null,
        string loggingCorrelationValue = "", CancellationToken cancellationToken = default)
    {
        var logContexts = new Dictionary<string, object>
        {
            {"MethodName", nameof(GetSvgAssetAsync)}
        };
        if (!string.IsNullOrWhiteSpace(_configuration.LoggingCorrelationIdentifier) &&
            !string.IsNullOrWhiteSpace(loggingCorrelationValue))
            logContexts.Add(_configuration.LoggingCorrelationIdentifier, loggingCorrelationValue);
        using (_logger?.BeginScope(logContexts.Concat(_baseLogContexts)))
        {
            // set configuration options
            var cache = useCache ?? _configuration.EnableSvgCache;
            var expiry = noExpiry ?? _configuration.NoExpiry;
            var useDefaultSvg = false;
            string? responseContent = null;

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                _logger?.LogDebug("Retrieving SVG asset: {Filename}", filename ?? "<filename not specified>");

                var filenameValidation = ValidateFilename(filename);
                if (!filenameValidation.isValidFilename)
                {
                    _logger?.LogWarning("Filename is invalid ({FilenameValidationMessage}), using default SVG",
                        filenameValidation.filenameErrorMessage);
                    useDefaultSvg = true;
                    responseContent = SvgResult.DefaultSvg;
                }

                responseContent ??= cache
                    ? (await GetCachedSvgAssetAsync(filename!, loggingCorrelationValue))?.Svg ?? null
                    : null;
                responseContent ??= _accessLocalFiles switch
                {
                    true => await GetSvgFromLocalFileAsync(filename!, loggingCorrelationValue, cancellationToken),
                    false => await GetSvgFromUpstreamAsync(filename!, loggingCorrelationValue, cancellationToken)
                };

                if (responseContent is null)
                {
                    _logger?.LogWarning("No SVG asset found for {Filename}, using default SVG", filename);
                    useDefaultSvg = true;
                    responseContent = SvgResult.DefaultSvg;
                }

                // isolate the SVG node within the response content
                _logger?.LogDebug("Extracting SVG node of asset {Filename}",
                    useDefaultSvg ? "<default SVG>" : filename ?? "<default SVG>");
                var svgXml = new XmlDocument();
                svgXml.LoadXml(responseContent);
                var svgNode = svgXml.GetElementsByTagName("svg").Item(0);
                if (svgNode is null)
                {
                    _logger?.LogWarning("Upstream asset {Filename} does not contain an SVG node, using default SVG",
                        filename);
                    useDefaultSvg = true;
                    svgNode = DefaultSvgXml();
                }

                // update cache unless disabled or using default SVG
                if (cache && !useDefaultSvg)
                    await UpdateCachedSvgAssetAsync(filename!, svgNode.OuterXml, expiry);

                // add attributes to SVG node and return result object
                var svgNodeWithAttributes = AddXmlAttributes(svgNode, attributes, useDefaultSvg ? null : filename,
                    loggingCorrelationValue);
                _logger?.LogDebug("Returning extracted SVG node from {Filename} with updated attributes",
                    useDefaultSvg ? "<default SVG>" : filename ?? "<default SVG>");
                return new SvgResult(svgNodeWithAttributes.OuterXml);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }
    }

    /// <summary>
    ///     Retrieve an SVG asset from the local file system.
    /// </summary>
    /// <param name="filename">Filename within the base directory to retrieve.</param>
    /// <param name="loggingCorrelationValue">Value to use for logging correlation. Default: empty string.</param>
    /// <param name="cancellationToken">Cancellation token. Default: none.</param>
    /// <returns>Received SVG as a string or null if any errors are encountered or the task is cancelled.</returns>
    private async ValueTask<string?> GetSvgFromLocalFileAsync(string filename, string loggingCorrelationValue = "",
        CancellationToken cancellationToken = default)
    {
        var logContexts = new Dictionary<string, object>
        {
            {"MethodName", nameof(GetSvgFromLocalFileAsync)}
        };
        if (!string.IsNullOrWhiteSpace(_configuration.LoggingCorrelationIdentifier) &&
            !string.IsNullOrWhiteSpace(loggingCorrelationValue))
            logContexts.Add(_configuration.LoggingCorrelationIdentifier, loggingCorrelationValue);
        using (_logger?.BeginScope(logContexts.Concat(_baseLogContexts)))
        {
            _logger?.LogDebug("Requesting {Filename} from local file system at {UpstreamAssetSource}", filename,
                _fileBaseDirectory);
            try
            {
                var fileContents =
                    await File.ReadAllTextAsync(Path.Combine(_fileBaseDirectory, filename), cancellationToken);
                if (fileContents.Length > 0 && !string.IsNullOrWhiteSpace(fileContents))
                    return fileContents;
                _logger?.LogWarning("Upstream asset {Filename} is empty or contains only whitespace", filename);
                return null;
            }
            catch (OperationCanceledException exception) when (cancellationToken.IsCancellationRequested)
            {
                _logger?.LogDebug(exception,
                    "Request for local asset {Filename} was cancelled: {ExceptionMessage}", filename,
                    exception.Message);
                return null;
            }
            catch (OperationCanceledException exception) when
                (exception.InnerException is TimeoutException timeoutException)
            {
                _logger?.LogWarning(timeoutException,
                    "Operation timed-out while retrieving local asset {Filename} from {UpstreamAssetSource}: {ExceptionMessage}",
                    filename, _fileBaseDirectory, timeoutException.Message);
                return null;
            }
            catch (FileNotFoundException exception)
            {
                _logger?.LogWarning(exception, "Local asset {Filename} from {UpstreamAssetSource} not found", filename,
                    _fileBaseDirectory);
                return null;
            }
            catch (UnauthorizedAccessException exception)
            {
                _logger?.LogError(exception,
                    "Access to local asset {Filename} from {UpstreamAssetSource} is not permitted", filename,
                    _fileBaseDirectory);
                return null;
            }
            catch (Exception exception)
            {
                _logger?.LogError(exception,
                    "Error while retrieving local asset {Filename} from {UpstreamAssetSource}: {ExceptionMessage}",
                    filename, _fileBaseDirectory, exception.Message);
                return null;
            }
        }
    }

    /// <summary>
    ///     Retrieve an SVG asset from a remote upstream source.
    /// </summary>
    /// <param name="filename">Filename to retrieve from the base URL.</param>
    /// <param name="loggingCorrelationValue">Value to use for logging correlation. Default: empty string.</param>
    /// <param name="cancellationToken">Cancellation token. Default: none.</param>
    /// <returns>Received SVG as a string or null if any errors are encountered or the task is cancelled.</returns>
    private async ValueTask<string?> GetSvgFromUpstreamAsync(string filename, string loggingCorrelationValue = "",
        CancellationToken cancellationToken = default)
    {
        var logContexts = new Dictionary<string, object>
        {
            {"MethodName", nameof(GetSvgFromUpstreamAsync)}
        };
        if (!string.IsNullOrWhiteSpace(_configuration.LoggingCorrelationIdentifier) &&
            !string.IsNullOrWhiteSpace(loggingCorrelationValue))
            logContexts.Add(_configuration.LoggingCorrelationIdentifier, loggingCorrelationValue);
        using (_logger?.BeginScope(logContexts.Concat(_baseLogContexts)))
        {
            _logger?.LogDebug("Requesting {Filename} from {UpstreamAssetSource}", filename, _httpBaseAddress);
            try
            {
                var httpClient = _httpClientFactory.CreateClient(_configuration.HttpClientName);
                httpClient.BaseAddress = _httpBaseAddress;
                httpClient.Timeout = _httpTimeout;

                using var httpRequest = new HttpRequestMessage(HttpMethod.Get, filename);
                httpRequest.Headers.Add("Accept", "image/svg+xml");
                var httpResponse = await httpClient.SendAsync(httpRequest, cancellationToken);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var fileContents = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
                    if (fileContents.Length > 0 && !string.IsNullOrWhiteSpace(fileContents))
                        return fileContents;
                    _logger?.LogWarning("Upstream asset {Filename} is empty or contains only whitespace", filename);
                    return null;
                }

                _logger?.LogWarning(
                    "Upstream asset {Filename} not found or not available: [{HttpStatusCode}] {HttpStatusMessage}",
                    filename, httpResponse.StatusCode, httpResponse.ReasonPhrase);
                return null;
            }
            catch (OperationCanceledException exception) when (cancellationToken.IsCancellationRequested)
            {
                _logger?.LogDebug(exception, "Request for upstream asset {Filename} was cancelled: {ExceptionMessage}",
                    filename, exception.Message);
                return null;
            }
            catch (OperationCanceledException exception) when
                (exception.InnerException is TimeoutException timeoutException)
            {
                _logger?.LogWarning(timeoutException,
                    "Operation timed-out while retrieving upstream asset {Filename} from {UpstreamAssetSource}: {ExceptionMessage}",
                    filename, _httpBaseAddress, timeoutException.Message);
                return null;
            }
            catch (Exception e)
            {
                _logger?.LogError(e,
                    "Error while retrieving upstream asset {Filename} from {UpstreamAssetSource}: {ExceptionMessage}",
                    filename, _httpBaseAddress, e.Message);
                return null;
            }
        }
    }

    /// <summary>
    ///     Retrieve an SVG asset from the cache, if available.
    /// </summary>
    /// <param name="filename">Filename identifier to search for in the cache.</param>
    /// <param name="loggingCorrelationValue">Value to use for log correlation. Default: empty string.</param>
    /// <returns>SvgCachedObject if found, null otherwise.</returns>
    private async ValueTask<SvgCachedObject?> GetCachedSvgAssetAsync(string filename,
        string loggingCorrelationValue = "")
    {
        var logContexts = new Dictionary<string, object>
        {
            {"MethodName", nameof(GetCachedSvgAssetAsync)}
        };
        if (!string.IsNullOrWhiteSpace(_configuration.LoggingCorrelationIdentifier) &&
            !string.IsNullOrWhiteSpace(loggingCorrelationValue))
            logContexts.Add(_configuration.LoggingCorrelationIdentifier, loggingCorrelationValue);
        using (_logger?.BeginScope(logContexts.Concat(_baseLogContexts)))
        {
            _logger?.LogDebug("Checking cache for SVG markup from {Filename}", filename);
            var cachedSvg = await _svgCache.GetCachedSvg(filename);
            _logger?.LogDebug("Received {CachedSvg} from cache", cachedSvg?.ToString() ?? "<null>");
            if (cachedSvg is not null && (cachedSvg.Expiry is null || cachedSvg.Expiry > DateTime.UtcNow))
            {
                _logger?.LogDebug("Returning cached SVG markup for {Filename}", filename);
                return cachedSvg;
            }

            _logger?.LogDebug("No valid cached SVG markup found for {Filename}", filename);
            return null;
        }
    }

    /// <summary>
    ///     Update or create an SvgCachedObject in the cache.
    /// </summary>
    /// <param name="filename">Filename identifier for this SVG object.</param>
    /// <param name="svg">SVG string to store in the cache.</param>
    /// <param name="noExpiry">If true, DO NOT set an expiry time for this cache entry. Default: false.</param>
    /// <param name="loggingCorrelationValue">Value to use for logging correlation. Default: empty string.</param>
    private async ValueTask UpdateCachedSvgAssetAsync(string filename, string svg, bool noExpiry = false,
        string loggingCorrelationValue = "")
    {
        var logContexts = new Dictionary<string, object>
        {
            {"MethodName", nameof(UpdateCachedSvgAssetAsync)}
        };
        if (!string.IsNullOrWhiteSpace(_configuration.LoggingCorrelationIdentifier) &&
            !string.IsNullOrWhiteSpace(loggingCorrelationValue))
            logContexts.Add(_configuration.LoggingCorrelationIdentifier, loggingCorrelationValue);
        using (_logger?.BeginScope(logContexts.Concat(_baseLogContexts)))
        {
            _logger?.LogDebug("Updating cached SVG markup for {Filename}", filename);
            var cacheResult =
                await _svgCache.RegisterAsync(filename, svg, noExpiry ? 0 : _configuration.SvgCacheExpiryMinutes);
            if (cacheResult.result)
                _logger?.LogDebug(
                    "Successfully cached SVG markup for {Filename} (valid for: {ExpiryTimeMinutes} minutes", filename,
                    noExpiry ? "infinite" : _configuration.SvgCacheExpiryMinutes.ToString());
            else
                _logger?.LogError(cacheResult.exception, "Error caching SVG markup for {Filename}", filename);
        }
    }

    /// <summary>
    ///     Create an XML document using the default SVG markup.
    /// </summary>
    /// <returns>XML document containing the default SVG.</returns>
    private static XmlDocument DefaultSvgXml()
    {
        return new XmlDocument {InnerXml = SvgResult.DefaultSvg};
    }

    /// <summary>
    ///     Add attributes to an XML node based on a list of key-value pairs.
    /// </summary>
    /// <param name="svgNode">XML representation of SVG to which attributes should be added.</param>
    /// <param name="attributes">Key-Value pairs from which to derive attributes.</param>
    /// <param name="filename">
    ///     Name of the file being processed. If null or omitted, the default value is used. Default: "default SVG".
    /// </param>
    /// <param name="loggingCorrelationValue">Value to use for logging correlation. Default: empty string.</param>
    /// <returns>XmlNode containing the modified SVG.</returns>
    private XmlNode AddXmlAttributes(XmlNode svgNode, List<KeyValuePair<string, string>>? attributes,
        string? filename = "<default SVG>", string loggingCorrelationValue = "")
    {
        var logContexts = new Dictionary<string, object>
        {
            {"MethodName", nameof(AddXmlAttributes)}
        };
        if (!string.IsNullOrWhiteSpace(_configuration.LoggingCorrelationIdentifier) &&
            !string.IsNullOrWhiteSpace(loggingCorrelationValue))
            logContexts.Add(_configuration.LoggingCorrelationIdentifier, loggingCorrelationValue);
        using (_logger?.BeginScope(logContexts.Concat(_baseLogContexts)))
        {
            if (attributes is null || attributes.Count == 0) return svgNode;

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(svgNode.OuterXml);
            var existingAttributes = svgNode.Attributes?.Cast<XmlAttribute>() ?? [];
            foreach (var attribute in existingAttributes)
                _logger?.LogDebug("Found existing XML attribute in {Filename}: {AttributeName} = {AttributeValue}",
                    filename, attribute.Name, attribute.Value);
            foreach (var (name, value) in attributes)
            {
                _logger?.LogDebug("Adding new XML attribute to {Filename}: {AttributeName} = {AttributeValue}",
                    filename, name, value);
                var newAttribute = xmlDoc.CreateAttribute(name);
                newAttribute.Value = value;
                svgNode.Attributes?.Append(newAttribute);
            }

            return svgNode;
        }
    }

    /// <summary>
    ///     Validate the provided upstream asset filename.
    /// </summary>
    /// <param name="filename">Filename to validate</param>
    /// <returns>True if valid, False with an error message otherwise</returns>
    private static (bool isValidFilename, string filenameErrorMessage) ValidateFilename(string? filename)
    {
        if (string.IsNullOrWhiteSpace(filename))
            return (false, "no filename specified");

        return (true, string.Empty);
    }
}