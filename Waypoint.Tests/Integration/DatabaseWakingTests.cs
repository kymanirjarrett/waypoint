using System.Net;
using Microsoft.EntityFrameworkCore.Storage;

namespace Waypoint.Tests.Integration;

public class DatabaseWakingTests
{
    [Fact]
    public async Task PausedDatabase_ShowsWakingPageThatReloadsItself()
    {
        using var factory = new WaypointFactory { QueryFailure = () => new RetryLimitExceededException("database resuming") };
        var client = factory.CreateHttpsClient();

        var response = await client.GetAsync("/destinations/table");
        var body = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);
        Assert.Equal("20", response.Headers.GetValues("Retry-After").Single());
        Assert.Equal("20; url=/destinations/table", response.Headers.GetValues("Refresh").Single());
        Assert.Contains("about a minute", body);
    }

    [Fact]
    public async Task OtherFailure_ShowsGenericErrorPage()
    {
        using var factory = new WaypointFactory { QueryFailure = () => new InvalidOperationException("bug") };
        var client = factory.CreateHttpsClient();

        var response = await client.GetAsync("/destinations/table");

        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        Assert.Contains("Something went wrong", await response.Content.ReadAsStringAsync());
    }
}
