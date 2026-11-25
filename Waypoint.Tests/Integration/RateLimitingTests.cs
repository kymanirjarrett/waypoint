using System.Net;

namespace Waypoint.Tests.Integration;

/// <summary>The per-IP limiter on database pages, with a limit of 3 per minute so it trips quickly.</summary>
public class RateLimitingTests : IDisposable
{
    private readonly WaypointFactory _factory = new() { PermitPerMinute = 3 };

    public void Dispose() => _factory.Dispose();

    // Each request uses a new continent value so the output cache never answers it: only database work is limited.
    private static async Task<HttpResponseMessage> GetUncached(HttpClient client, string clientIp, int n)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"/destinations?continent=probe-{clientIp}-{n}");
        request.Headers.Add("X-Forwarded-For", clientIp);
        return await client.SendAsync(request);
    }

    [Fact]
    public async Task DatabasePage_OverLimit_Returns429WithRetryAfter()
    {
        var client = _factory.CreateHttpsClient();

        for (var i = 0; i < 3; i++)
        {
            Assert.Equal(HttpStatusCode.OK, (await GetUncached(client, "203.0.113.10", i)).StatusCode);
        }

        var limited = await GetUncached(client, "203.0.113.10", 3);
        Assert.Equal(HttpStatusCode.TooManyRequests, limited.StatusCode);
        Assert.True(limited.Headers.Contains("Retry-After"));
        Assert.Contains("Slow down", await limited.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task Limit_IsPerClientIp()
    {
        var client = _factory.CreateHttpsClient();

        for (var i = 0; i < 4; i++)
        {
            await GetUncached(client, "203.0.113.20", i);
        }

        Assert.Equal(HttpStatusCode.OK, (await GetUncached(client, "198.51.100.7", 0)).StatusCode);
    }

    [Fact]
    public async Task StaticPages_AreNotLimited()
    {
        var client = _factory.CreateHttpsClient();

        for (var i = 0; i < 6; i++)
        {
            var response = await client.GetAsync("/about");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
