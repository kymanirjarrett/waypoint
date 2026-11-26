using System.Threading.RateLimiting;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using OpenTelemetry.Logs;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Waypoint;
using Waypoint.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHealthChecks();

// Production telemetry: requests, dependencies (SQL), exceptions, and warning-or-worse logs go to Application Insights.
// App Service supplies the connection string as an app setting; locally it is unset, so nothing is sent.
if (!string.IsNullOrWhiteSpace(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]))
{
    builder.Services.AddOpenTelemetry().UseAzureMonitor();
    builder.Logging.AddFilter<OpenTelemetryLoggerProvider>(null, LogLevel.Warning);
}

// Register the EF Core DbContext so controllers can receive it through constructor injection.
// The connection string lives in appsettings.json and points at SQL Server LocalDB.
var connectionString = builder.Configuration.GetConnectionString("WaypointDb")
    ?? throw new InvalidOperationException("Connection string 'WaypointDb' not found.");
if (builder.Environment.IsProduction())
{
    // A resuming Azure SQL database stalls the login until the connect timeout (default 15 s) expires.
    // Cap it at 10 s so visitors reach the "Waking up" page quickly instead of a long blank load.
    connectionString = new SqlConnectionStringBuilder(connectionString) { ConnectTimeout = 10 }.ConnectionString;
}

builder.Services.AddDbContext<WaypointDbContext>(options =>
    options.UseSqlServer(connectionString,
        // Retry briefly (~10 s) for ordinary blips. A paused free database takes about a minute to resume, so rather than
        // hold the visitor on a blank loading screen, give up early and show the "Waking up" page, which reloads itself.
        sqlOptions => sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 3,
            maxRetryDelay: TimeSpan.FromSeconds(4),
            errorNumbersToAdd: null))
        // The site only reads data, so skip change tracking for every query by default.
        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

// Destination data only changes with a deploy (seed data + migration), and a deploy restarts the app,
// which clears this in-memory cache. So cached pages never go stale and visitors rarely wake the paused database.
builder.Services.AddOutputCache(options =>
    options.AddPolicy("Destinations", policy => policy.Expire(TimeSpan.FromMinutes(10)).SetVaryByQuery("continent", "sort")));

// App Service terminates TLS at its front end and appends the real client IP to X-Forwarded-For.
// Traffic can only reach the app through that front end, so trust the header (ForwardLimit 1 = the entry it appended).
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownIPNetworks.Clear();
    options.KnownProxies.Clear();
});

// Per-IP limit on database-backed pages so one client cannot drain the free database allowance.
// Static pages are exempt; see docs/adr/0001-rate-limiting.md.
var permitPerMinute = builder.Configuration.GetValue("RateLimiting:PermitPerMinute", 60);
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.OnRejected = (context, _) =>
    {
        if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
        {
            context.HttpContext.Response.Headers.RetryAfter = ((int)retryAfter.TotalSeconds).ToString();
        }

        return ValueTask.CompletedTask;
    };
    options.AddPolicy(RateLimits.PerIp, httpContext => RateLimitPartition.GetFixedWindowLimiter(
        httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
        _ => new FixedWindowRateLimiterOptions { PermitLimit = permitPerMinute, Window = TimeSpan.FromMinutes(1) }));
});

// Generate lowercase URLs from tag helpers and Url.Action (e.g. /about/kymani, not /About/Kymani).
builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

var app = builder.Build();

// First, so every later middleware (rate limiter, HTTPS redirect, logging) sees the real client IP and scheme.
app.UseForwardedHeaders();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    // In development, create the LocalDB database and apply any pending migrations on startup
    // so a fresh clone runs without a manual `dotnet ef database update`.
    await using var scope = app.Services.CreateAsyncScope();
    await scope.ServiceProvider.GetRequiredService<WaypointDbContext>().Database.MigrateAsync();
}

// Empty error responses (like NotFound() for an unknown slug) re-run through a styled page instead of a blank browser 404.
app.UseStatusCodePagesWithReExecute("/status/{0}");

// Baseline security headers on every response. Everything (scripts, styles, fonts, images) is self-hosted,
// so the policy only allows this origin; frame-ancestors 'none' blocks clickjacking.
app.Use(async (context, next) =>
{
    var headers = context.Response.Headers;
    headers.XContentTypeOptions = "nosniff";
    headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
    headers.ContentSecurityPolicy =
        "default-src 'self'; img-src 'self' data:; object-src 'none'; base-uri 'self'; form-action 'self'; frame-ancestors 'none'";
    await next();
});

app.UseHttpsRedirection();
app.UseRouting();
// Cached responses are served before the limiter, so cheap repeat views never count against a client.
app.UseOutputCache();
app.UseRateLimiter();

app.UseAuthorization();

app.MapStaticAssets();

// Liveness only: no database check, so uptime probes and the deploy smoke test never wake the paused database.
app.MapHealthChecks("/healthz");

// Every controller declares its routes with [Route] attributes, so no conventional route table is needed.
app.MapControllers()
    .WithStaticAssets();


app.Run();
