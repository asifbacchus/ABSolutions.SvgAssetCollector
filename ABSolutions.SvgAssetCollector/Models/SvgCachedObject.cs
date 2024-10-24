namespace ABSolutions.SvgAssetCollector.Models;

/// <summary>
///     SVG cached object with optional expiry date.
/// </summary>
public record SvgCachedObject
{
    public required string Filename { get; init; }
    public required string Svg { get; init; }
    public DateTime? Expiry { get; init; }

    /// <summary>
    ///     Override equality operator: Only compare Filename property. Ignore other properties.
    /// </summary>
    /// <param name="other">SvgCachedObject to compare against this one.</param>
    /// <returns>True if Filename property is equal, False otherwise.</returns>
    public virtual bool Equals(SvgCachedObject? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Filename == other.Filename;
    }

    /// <summary>
    ///     Override ToString() to provide relevant debugging output.
    /// </summary>
    /// <returns>String representation of this object.</returns>
    public override string ToString()
    {
        return
            $"Filename: {Filename} | SVG: {(string.IsNullOrWhiteSpace(Svg) ? "<null>" : "<has value>")} | Expiry: {(Expiry.HasValue ? Expiry.Value.ToString("O") : "infinite")}";
    }

    /// <summary>
    ///     Override hash code generation: Only use Filename property. Ignore other properties.
    /// </summary>
    /// <returns>Integer hash of the Filename property.</returns>
    public override int GetHashCode()
    {
        return Filename.GetHashCode();
    }
}