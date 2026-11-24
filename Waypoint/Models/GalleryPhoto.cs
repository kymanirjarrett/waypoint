namespace Waypoint.Models;

/// <summary>One photo in the gallery grid (Views/Shared/_PhotoGrid.cshtml).</summary>
/// <param name="Src">App-relative path, e.g. "/images/gallery/kyoto-lanterns.webp".</param>
/// <param name="Alt">What the photo shows, for screen readers.</param>
/// <param name="Caption">Short caption shown under the photo.</param>
/// <param name="Credit">Who took it. Leave null for team photos.</param>
/// <param name="CreditUrl">Source page for a stock photo's license.</param>
public record GalleryPhoto(string Src, string Alt, string Caption, string? Credit = null, string? CreditUrl = null);
