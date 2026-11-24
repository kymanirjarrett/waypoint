using Microsoft.EntityFrameworkCore;
using Waypoint.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register the EF Core DbContext so controllers can receive it through constructor injection.
// The connection string lives in appsettings.json and points at SQL Server LocalDB.
builder.Services.AddDbContext<WaypointDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("WaypointDb")
            ?? throw new InvalidOperationException("Connection string 'WaypointDb' not found."),
        // The Azure SQL free database auto-pauses when idle; the first query after that can fail transiently while it resumes.
        sqlOptions => sqlOptions.EnableRetryOnFailure()));

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

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

// Every controller declares its routes with [Route] attributes, so no conventional route table is needed.
app.MapControllers()
    .WithStaticAssets();


app.Run();
