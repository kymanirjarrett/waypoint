using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.OutputCaching;
using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Waypoint.Data;
using Waypoint.Models;

namespace Waypoint.Controllers;

public class HomeController(WaypointDbContext db) : Controller
{
    /// <summary>Number of destinations previewed on the home page.</summary>
    public const int PreviewCount = 4;

    [OutputCache(PolicyName = "Destinations")]
    [EnableRateLimiting(RateLimits.PerIp)]
    [Route("")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
    {
        // The top-rated place becomes the hero photo; the next ones are previewed below it.
        var topRated = await db.Destinations
            .OrderByDescending(d => d.Rating)
            .ThenBy(d => d.Name)
            .Take(PreviewCount + 1)
            .ToListAsync(cancellationToken);

        var count = await db.Destinations.CountAsync(cancellationToken);
        return View(new HomeIndexViewModel(topRated.FirstOrDefault(), topRated.Skip(1).ToList(), count));
    }

    [Route("privacy")]
    public IActionResult Privacy() => View();

    [Route("gallery")]
    public IActionResult Gallery() => View();

    [Route("tips")]
    public IActionResult Tips() => View();

    [Route("error")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        var feature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

        // The free database pauses after an hour idle. Instead of a generic error, say it is waking up,
        // answer 503 so crawlers treat it as temporary, and reload the page the visitor asked for.
        if (DatabaseWake.IsWaking(feature?.Error))
        {
            var path = Url.IsLocalUrl(feature?.Path) ? feature!.Path : "/";
            Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            Response.Headers.RetryAfter = DatabaseWake.RetryAfterSeconds.ToString();
            Response.Headers["Refresh"] = $"{DatabaseWake.RetryAfterSeconds}; url={path}";
            return View("Waking", path);
        }

        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    /// <summary>Re-executed by UseStatusCodePagesWithReExecute for empty 4xx/5xx responses, such as an unknown slug.</summary>
    [Route("status/{code:int:range(400,599)}")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult StatusCodePage(int code)
    {
        Response.StatusCode = code;
        return View(code == StatusCodes.Status404NotFound ? "NotFound" : "StatusCode", code);
    }
}
