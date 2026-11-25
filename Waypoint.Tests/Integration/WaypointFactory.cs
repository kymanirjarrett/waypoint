using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Waypoint.Data;

namespace Waypoint.Tests.Integration;

/// <summary>
/// Runs the real app (middleware, routing, views) in memory, with the SQL Server DbContext swapped for EF InMemory.
/// Environment is "Testing", so the production error handler and status pages are active and no migrations run.
/// </summary>
public class WaypointFactory : WebApplicationFactory<Program>
{
    private readonly string _databaseName = Guid.NewGuid().ToString();

    /// <summary>Requests per minute per client IP. High by default so ordinary tests never hit the limiter.</summary>
    public int PermitPerMinute { get; init; } = 10_000;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.UseSetting("ConnectionStrings:WaypointDb", "unused-in-tests");
        builder.UseSetting("RateLimiting:PermitPerMinute", PermitPerMinute.ToString());

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<WaypointDbContext>>();
            services.RemoveAll<IDbContextOptionsConfiguration<WaypointDbContext>>();
            services.AddDbContext<WaypointDbContext>(options => options.UseInMemoryDatabase(_databaseName));

            using var scope = services.BuildServiceProvider().CreateScope();
            scope.ServiceProvider.GetRequiredService<WaypointDbContext>().Database.EnsureCreated(); // seed rows
        });
    }

    /// <summary>An HTTPS client that does not follow redirects, so status codes are asserted as sent.</summary>
    public HttpClient CreateHttpsClient() =>
        CreateClient(new WebApplicationFactoryClientOptions { BaseAddress = new Uri("https://localhost"), AllowAutoRedirect = false });
}
