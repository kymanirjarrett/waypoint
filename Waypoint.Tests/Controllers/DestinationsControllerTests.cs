using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Waypoint.Controllers;
using Waypoint.Data;
using Waypoint.Models;

namespace Waypoint.Tests.Controllers;

public class DestinationsControllerTests : IDisposable
{
    private readonly WaypointDbContext _db;
    private readonly DestinationsController _controller;

    public DestinationsControllerTests()
    {
        var options = new DbContextOptionsBuilder<WaypointDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _db = new WaypointDbContext(options);
        _db.Database.EnsureCreated(); // applies the HasData seed rows
        _controller = new DestinationsController(_db);
    }

    public void Dispose() => _db.Dispose();

    [Fact]
    public async Task Index_ReturnsAllDestinationsSortedByName()
    {
        var view = Assert.IsType<ViewResult>(await _controller.Index(null));
        var model = Assert.IsAssignableFrom<IEnumerable<Destination>>(view.Model).ToList();

        Assert.Equal(SeedData.Destinations.Length, model.Count);
        Assert.Equal(model.OrderBy(d => d.Name).Select(d => d.Name), model.Select(d => d.Name));
    }

    [Fact]
    public async Task Index_FiltersByContinent()
    {
        var view = Assert.IsType<ViewResult>(await _controller.Index("Europe"));
        var model = Assert.IsAssignableFrom<IEnumerable<Destination>>(view.Model).ToList();

        Assert.NotEmpty(model);
        Assert.All(model, d => Assert.Equal("Europe", d.Continent));
    }

    [Fact]
    public async Task Details_KnownSlug_ReturnsDestination()
    {
        var view = Assert.IsType<ViewResult>(await _controller.Details("kyoto"));
        var model = Assert.IsType<Destination>(view.Model);

        Assert.Equal("Kyoto", model.Name);
    }

    [Fact]
    public async Task Details_SetsNextDestinationAlphabetically()
    {
        var view = Assert.IsType<ViewResult>(await _controller.Details("kyoto"));
        var next = Assert.IsType<Destination>(view.ViewData["Next"]);

        Assert.Equal("Lisbon", next.Name);
    }

    [Fact]
    public async Task Details_LastDestination_WrapsToFirst()
    {
        var last = SeedData.Destinations.OrderBy(d => d.Name).Last();
        var first = SeedData.Destinations.OrderBy(d => d.Name).First();

        var view = Assert.IsType<ViewResult>(await _controller.Details(last.Slug));

        Assert.Equal(first.Slug, Assert.IsType<Destination>(view.ViewData["Next"]).Slug);
    }

    [Fact]
    public async Task Details_UnknownSlug_ReturnsNotFound() =>
        Assert.IsType<NotFoundResult>(await _controller.Details("atlantis"));

    [Fact]
    public async Task Table_SortsByRatingDescending()
    {
        var view = Assert.IsType<ViewResult>(await _controller.Table("rating"));
        var ratings = Assert.IsAssignableFrom<IEnumerable<Destination>>(view.Model).Select(d => d.Rating).ToList();

        Assert.Equal(ratings.OrderByDescending(r => r), ratings);
        Assert.Equal("rating", view.ViewData["Sort"]);
    }

    [Fact]
    public async Task Table_UnknownSort_FallsBackToName()
    {
        var view = Assert.IsType<ViewResult>(await _controller.Table("bogus"));

        Assert.Equal("name", view.ViewData["Sort"]);
    }

    [Fact]
    public void SeedData_SlugsAreUnique() =>
        Assert.Equal(SeedData.Destinations.Length, SeedData.Destinations.Select(d => d.Slug).Distinct().Count());
}
