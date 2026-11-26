namespace Waypoint;

/// <summary>Rate limiter policy names, shared by Program.cs and the [EnableRateLimiting] attributes.</summary>
public static class RateLimits
{
    /// <summary>Per-client-IP fixed window on pages that query the database. See docs/adr/0001-rate-limiting.md.</summary>
    public const string PerIp = "per-ip";
}
