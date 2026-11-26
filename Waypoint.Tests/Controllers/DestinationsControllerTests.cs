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
        var model = Assert.IsType<DestinationIndexViewModel>(view.Model).Destinations;

        Assert.Equal(SeedData.Destinations.Length, model.Count);
        Assert.Equal(model.OrderBy(d => d.Name).Select(d => d.Name), model.Select(d => d.Name));
    }

    [Fact]
    public async Task Index_FiltersByContinent()
    {
        var view = Assert.IsType<ViewResult>(await _controller.Index("Europe"));
        var model = Assert.IsType<DestinationIndexViewModel>(view.Model).Destinations;

        Assert.NotEmpty(model);
        Assert.All(model, d => Assert.Equal("Europe", d.Continent));
    }

    [Fact]
    public async Task Details_KnownSlug_ReturnsDestination()
    {
        var view = Assert.IsType<ViewResult>(await _controller.Details("kyoto"));
        var model = Assert.IsType<DestinationDetailsViewModel>(view.Model).Destination;

        Assert.Equal("Kyoto", model.Name);
    }

    [Fact]
    public async Task Details_SetsNextDestinationAlphabetically()
    {
        var byName = SeedData.Destinations.OrderBy(d => d.Name).ToList();

        var view = Assert.IsType<ViewResult>(await _controller.Details(byName[0].Slug));
        var next = Assert.IsType<Destination>(Assert.IsType<DestinationDetailsViewModel>(view.Model).Next);

        Assert.Equal(byName[1].Name, next.Name);
    }

    [Fact]
    public async Task Details_LastDestination_WrapsToFirst()
    {
        var last = SeedData.Destinations.OrderBy(d => d.Name).Last();
        var first = SeedData.Destinations.OrderBy(d => d.Name).First();

        var view = Assert.IsType<ViewResult>(await _controller.Details(last.Slug));

        Assert.Equal(first.Slug, Assert.IsType<Destination>(Assert.IsType<DestinationDetailsViewModel>(view.Model).Next).Slug);
    }

    [Fact]
    public async Task Details_UnknownSlug_ReturnsNotFound() =>
        Assert.IsType<NotFoundResult>(await _controller.Details("atlantis"));

    [Fact]
    public async Task Table_SortsByRatingDescending()
    {
        var view = Assert.IsType<ViewResult>(await _controller.Table("rating"));
        var ratings = Assert.IsType<DestinationTableViewModel>(view.Model).Destinations.Select(d => d.Rating).ToList();

        Assert.Equal(ratings.OrderByDescending(r => r), ratings);
        Assert.Equal("rating", Assert.IsType<DestinationTableViewModel>(view.Model).Sort);
    }

    [Fact]
    public async Task Table_UnknownSort_FallsBackToName()
    {
        var view = Assert.IsType<ViewResult>(await _controller.Table("bogus"));

        Assert.Equal("name", Assert.IsType<DestinationTableViewModel>(view.Model).Sort);
    }

    public static TheoryData<string> OtherSortKeys() => new("country", "continent", "budget");

    [Theory]
    [MemberData(nameof(OtherSortKeys))]
    public async Task Table_SortsByEachKeyThenName(string key)
    {
        var view = Assert.IsType<ViewResult>(await _controller.Table(key));
        var model = Assert.IsType<DestinationTableViewModel>(view.Model).Destinations;

        var expected = key switch
        {
            "country" => SeedData.Destinations.OrderBy(d => d.Country).ThenBy(d => d.Name),
            "continent" => SeedData.Destinations.OrderBy(d => d.Continent).ThenBy(d => d.Name),
            _ => SeedData.Destinations.OrderBy(d => d.AverageDailyBudgetUsd).ThenBy(d => d.Name),
        };
        Assert.Equal(key, Assert.IsType<DestinationTableViewModel>(view.Model).Sort);
        Assert.Equal(expected.Select(d => d.Slug), model.Select(d => d.Slug));
    }

    [Fact]
    public async Task Details_MixedCaseSlug_ReturnsDestination()
    {
        var view = Assert.IsType<ViewResult>(await _controller.Details("KyOtO"));

        Assert.Equal("kyoto", Assert.IsType<DestinationDetailsViewModel>(view.Model).Destination.Slug);
    }

    [Fact]
    public async Task Details_OverlongSlug_ReturnsNotFound() =>
        Assert.IsType<NotFoundResult>(await _controller.Details(new string('a', 101)));

    [Fact]
    public async Task Details_NextIsNeverTheSameDestination()
    {
        foreach (var d in SeedData.Destinations)
        {
            var view = Assert.IsType<ViewResult>(await _controller.Details(d.Slug));
            Assert.NotEqual(d.Slug, Assert.IsType<Destination>(Assert.IsType<DestinationDetailsViewModel>(view.Model).Next).Slug);
        }
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task Index_BlankContinent_ReturnsAll(string continent)
    {
        var view = Assert.IsType<ViewResult>(await _controller.Index(continent));

        Assert.Equal(SeedData.Destinations.Length, Assert.IsType<DestinationIndexViewModel>(view.Model).Destinations.Count);
    }

    [Fact]
    public async Task Index_UnknownContinent_ReturnsEmptyList()
    {
        var view = Assert.IsType<ViewResult>(await _controller.Index("Atlantis"));

        Assert.Empty(Assert.IsType<DestinationIndexViewModel>(view.Model).Destinations);
        Assert.Equal("Atlantis", Assert.IsType<DestinationIndexViewModel>(view.Model).SelectedContinent);
    }

    [Fact]
    public async Task Index_ContinentList_IsDistinctAndSorted()
    {
        var view = Assert.IsType<ViewResult>(await _controller.Index(null));
        var continents = Assert.IsType<DestinationIndexViewModel>(view.Model).Continents;

        Assert.Equal(SeedData.Destinations.Select(d => d.Continent).Distinct().OrderBy(c => c), continents);
    }
}
