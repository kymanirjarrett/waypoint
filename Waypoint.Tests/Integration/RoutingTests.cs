using System.Net;

namespace Waypoint.Tests.Integration;

/// <summary>End-to-end HTTP tests: real routing, attribute routes, status-code re-execution, and response headers.</summary>
public class RoutingTests(WaypointFactory factory) : IClassFixture<WaypointFactory>
{
    private readonly HttpClient _client = factory.CreateHttpsClient();

    [Theory]
    [InlineData("/")]
    [InlineData("/destinations")]
    [InlineData("/destinations?continent=Europe")]
    [InlineData("/destinations/table")]
    [InlineData("/destinations/table?sort=bogus")]
    [InlineData("/destinations/kyoto")]
    [InlineData("/destinations/KYOTO")]
    [InlineData("/gallery")]
    [InlineData("/tips")]
    [InlineData("/about")]
    [InlineData("/about/kymani")]
    [InlineData("/privacy")]
    public async Task Page_ReturnsOk(string path)
    {
        var response = await _client.GetAsync(path);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Theory]
    [InlineData("/destinations/atlantis")]
    [InlineData("/no-such-page")]
    public async Task UnknownPage_ReturnsStyled404(string path)
    {
        var response = await _client.GetAsync(path);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Contains("Off the map", await response.Content.ReadAsStringAsync());
    }

    [Theory]
    [InlineData("/status/200")]
    [InlineData("/status/99999")]
    [InlineData("/status/abc")]
    public async Task StatusRoute_OutsideErrorRange_IsNotFound(string path)
    {
        var response = await _client.GetAsync(path);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task HealthCheck_ReportsHealthy()
    {
        var response = await _client.GetAsync("/healthz");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Healthy", await response.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task Responses_CarrySecurityHeaders()
    {
        var response = await _client.GetAsync("/");

        Assert.Equal("nosniff", response.Headers.GetValues("X-Content-Type-Options").Single());
        Assert.Contains("frame-ancestors 'none'", response.Headers.GetValues("Content-Security-Policy").Single());
        Assert.False(response.Headers.Contains("Set-Cookie"));
    }

    [Fact]
    public async Task ErrorPage_ShowsNoDeveloperDetails()
    {
        var body = await _client.GetStringAsync("/error");

        Assert.Contains("Something went wrong", body);
        Assert.DoesNotContain("Development", body);
    }
}
