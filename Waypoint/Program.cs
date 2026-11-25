using Microsoft.EntityFrameworkCore;
using Waypoint.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHealthChecks();

// Register the EF Core DbContext so controllers can receive it through constructor injection.
// The connection string lives in appsettings.json and points at SQL Server LocalDB.
builder.Services.AddDbContext<WaypointDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("WaypointDb")
            ?? throw new InvalidOperationException("Connection string 'WaypointDb' not found."),
        // The Azure SQL free database auto-pauses after an hour idle and takes about a minute to resume.
        // Budget: 8 retries with backoff capped at 15 s is ~86 s of waiting; plus 9 connection attempts at the
        // 15 s default Connect Timeout is ~221 s worst case, under App Service's 230 s front-end timeout.
        sqlOptions => sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 8,
            maxRetryDelay: TimeSpan.FromSeconds(15),
            errorNumbersToAdd: null))
        // The site only reads data, so skip change tracking for every query by default.
        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

// Destination data only changes with a deploy (seed data + migration), and a deploy restarts the app,
// which clears this in-memory cache. So cached pages never go stale and visitors rarely wake the paused database.
builder.Services.AddOutputCache(options =>
    options.AddPolicy("Destinations", policy => policy.Expire(TimeSpan.FromMinutes(10)).SetVaryByQuery("continent", "sort")));

// Generate lowercase URLs from tag helpers and Url.Action (e.g. /about/kymani, not /About/Kymani).
builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

var app = builder.Build();

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
    using var scope = app.Services.CreateScope();
    scope.ServiceProvider.GetRequiredService<WaypointDbContext>().Database.Migrate();
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
app.UseOutputCache();

app.UseAuthorization();

app.MapStaticAssets();

// Liveness only: no database check, so uptime probes and the deploy smoke test never wake the paused database.
app.MapHealthChecks("/healthz");

// Every controller declares its routes with [Route] attributes, so no conventional route table is needed.
app.MapControllers()
    .WithStaticAssets();


app.Run();
