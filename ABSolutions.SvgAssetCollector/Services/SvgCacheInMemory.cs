using ABSolutions.SvgAssetCollector.Models;

namespace ABSolutions.SvgAssetCollector.Services;

public class SvgCacheInMemory : ISvgCache
{
    private readonly List<SvgCachedObject> _svgCachedObjects = [];

    public async ValueTask<(bool result, Exception? exception)> RegisterAsync(string filename, string svg,
        int expiryMinutes)
    {
        // delete existing object
        var existingObject = _svgCachedObjects.FirstOrDefault(x => x.Filename == filename);
        if (existingObject is not null) _svgCachedObjects.Remove(existingObject);

        // register new object
        try
        {
            _svgCachedObjects.Add(new SvgCachedObject
            {
                Filename = filename,
                Svg = svg,
                Expiry = expiryMinutes == 0 ? null : DateTime.UtcNow.AddMinutes(expiryMinutes)
            });
            return await ValueTask.FromResult<(bool, Exception?)>((true, null));
        }
        catch (Exception exception)
        {
            return await ValueTask.FromResult<(bool, Exception?)>((false, exception));
        }
    }

    public async ValueTask<SvgCachedObject?> GetCachedSvg(string filename)
    {
        return await ValueTask.FromResult(_svgCachedObjects.FirstOrDefault(x => x.Filename == filename));
    }
}