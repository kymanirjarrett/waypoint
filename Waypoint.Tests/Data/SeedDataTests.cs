using Waypoint.Data;

namespace Waypoint.Tests.Data;

public class SeedDataTests
{
    // Walk up from the test output folder to the repo root (the folder holding Waypoint.slnx).
    private static readonly string WebRoot = FindWebRoot();

    private static string FindWebRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null && !File.Exists(Path.Combine(dir.FullName, "Waypoint.slnx")))
        {
            dir = dir.Parent;
        }

        return Path.Combine(dir?.FullName ?? throw new DirectoryNotFoundException("Waypoint.slnx not found"), "Waypoint", "wwwroot");
    }

    public static TheoryData<string> Slugs() => new(SeedData.Destinations.Select(d => d.Slug));

    [Theory]
    [MemberData(nameof(Slugs))]
    public void EverySeedRow_HasPhotoAltTextAndCredit(string slug)
    {
        var d = SeedData.Destinations.Single(x => x.Slug == slug);

        Assert.False(string.IsNullOrWhiteSpace(d.ImageAlt));
        Assert.False(string.IsNullOrWhiteSpace(d.ImageCredit));
        Assert.StartsWith("https://", d.ImageCreditUrl);
        Assert.StartsWith("/images/destinations/", d.ImagePath);
        Assert.True(File.Exists(Path.Combine(WebRoot, d.ImagePath.TrimStart('/'))), $"Missing photo file for {slug}");
    }
}
