using Microsoft.AspNetCore.OutputCaching;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Waypoint.Data;
using Waypoint.Models;

namespace Waypoint.Controllers;

[Route("destinations")]
public class DestinationsController(WaypointDbContext db) : Controller
{
    /// <summary>Card grid of every destination, optionally filtered by continent.</summary>
    [OutputCache(PolicyName = "Destinations")]
    [Route("")]
    public async Task<IActionResult> Index(string? continent, CancellationToken cancellationToken = default)
    {
        ViewData["Continents"] = await db.Destinations
            .Select(d => d.Continent)
            .Distinct()
            .OrderBy(c => c)
            .ToListAsync(cancellationToken);
        ViewData["SelectedContinent"] = continent;

        IQueryable<Destination> query = db.Destinations;
        if (!string.IsNullOrWhiteSpace(continent))
        {
            query = query.Where(d => d.Continent == continent);
        }

        return View(await query.OrderBy(d => d.Name).ToListAsync(cancellationToken));
    }

    /// <summary>Every row in the Destinations table, shown as an HTML table.</summary>
    [OutputCache(PolicyName = "Destinations")]
    [Route("table")]
    public async Task<IActionResult> Table(string? sort, CancellationToken cancellationToken = default)
    {
        var destinations = db.Destinations;
        (var query, sort) = sort switch
        {
            "country" => (destinations.OrderBy(d => d.Country).ThenBy(d => d.Name), sort),
            "continent" => (destinations.OrderBy(d => d.Continent).ThenBy(d => d.Name), sort),
            "budget" => (destinations.OrderBy(d => d.AverageDailyBudgetUsd).ThenBy(d => d.Name), sort),
            "rating" => (destinations.OrderByDescending(d => d.Rating).ThenBy(d => d.Name), sort),
            _ => (destinations.OrderBy(d => d.Name), "name"),
        };

        ViewData["Sort"] = sort;
        return View(await query.ToListAsync(cancellationToken));
    }

    /// <summary>Detail page for one destination. Unknown slugs return 404.</summary>
    [OutputCache(PolicyName = "Destinations")]
    [Route("{slug}")]
    public async Task<IActionResult> Details(string slug, CancellationToken cancellationToken = default)
    {
        // Slugs are stored lowercase (max 100 chars). ToLowerInvariant avoids culture surprises like the Turkish dotless i.
        if (slug.Length > 100)
        {
            return NotFound();
        }

        var key = slug.ToLowerInvariant();
        var destination = await db.Destinations
            .FirstOrDefaultAsync(d => d.Slug == key, cancellationToken);

        if (destination is null)
        {
            return NotFound();
        }

        // "Next destination" link: the next one alphabetically, wrapping back to the first.
        ViewData["Next"] = await db.Destinations
                               .Where(d => string.Compare(d.Name, destination.Name) > 0)
                               .OrderBy(d => d.Name)
                               .FirstOrDefaultAsync(cancellationToken)
                           ?? await db.Destinations
                               .Where(d => d.Id != destination.Id)
                               .OrderBy(d => d.Name)
                               .FirstOrDefaultAsync(cancellationToken);

        return View(destination);
    }
}
