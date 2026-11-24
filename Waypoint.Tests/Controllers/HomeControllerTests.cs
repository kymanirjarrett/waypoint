using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Waypoint.Controllers;
using Waypoint.Data;
using Waypoint.Models;

namespace Waypoint.Tests.Controllers;

public class HomeControllerTests : IDisposable
{
    private readonly WaypointDbContext _db;
    private readonly HomeController _controller;

    public HomeControllerTests()
    {
        var options = new DbContextOptionsBuilder<WaypointDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _db = new WaypointDbContext(options);
        _db.Database.EnsureCreated(); // applies the HasData seed rows
        _controller = new HomeController(_db)
        {
            ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() },
        };
    }

    public void Dispose() => _db.Dispose();

    [Fact]
    public async Task Index_PreviewsTopRatedDestinationsFromDatabase()
    {
        var view = Assert.IsType<ViewResult>(await _controller.Index());
        var model = Assert.IsAssignableFrom<IEnumerable<Destination>>(view.Model).ToList();

        Assert.Equal(HomeController.PreviewCount + 1, model.Count); // hero + preview
        Assert.Equal(model.OrderByDescending(d => d.Rating).ThenBy(d => d.Name).Select(d => d.Slug), model.Select(d => d.Slug));
        Assert.Equal(SeedData.Destinations.Length, view.ViewData["DestinationCount"]);
    }

    [Fact]
    public void Privacy_ReturnsView() => Assert.IsType<ViewResult>(_controller.Privacy());

    [Fact]
    public void Gallery_ReturnsView() => Assert.IsType<ViewResult>(_controller.Gallery());

    [Fact]
    public void Tips_ReturnsView() => Assert.IsType<ViewResult>(_controller.Tips());

    [Fact]
    public void StatusCodePage_404_UsesNotFoundViewAndKeepsStatus()
    {
        var view = Assert.IsType<ViewResult>(_controller.StatusCodePage(404));

        Assert.Equal("NotFound", view.ViewName);
        Assert.Equal(404, _controller.Response.StatusCode);
    }

    [Fact]
    public void StatusCodePage_OtherCode_UsesGenericView()
    {
        var view = Assert.IsType<ViewResult>(_controller.StatusCodePage(500));

        Assert.Equal("StatusCode", view.ViewName);
        Assert.Equal(500, _controller.Response.StatusCode);
    }
}
