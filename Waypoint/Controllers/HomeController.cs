using Microsoft.AspNetCore.OutputCaching;
using System.Diagnostics;
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

    [Route("")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
    {
        // The top-rated place becomes the hero photo; the next ones are previewed below it.
        var preview = await db.Destinations
            .OrderByDescending(d => d.Rating)
            .ThenBy(d => d.Name)
            .Take(PreviewCount + 1)
            .ToListAsync(cancellationToken);

        ViewData["DestinationCount"] = await db.Destinations.CountAsync(cancellationToken);
        return View(preview);
    }

    [Route("privacy")]
    public IActionResult Privacy()
    {
        return View();
    }

    [Route("gallery")]
    public IActionResult Gallery()
    {
        return View();
    }

    [Route("tips")]
    public IActionResult Tips()
    {
        return View();
    }

    [Route("error")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
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
