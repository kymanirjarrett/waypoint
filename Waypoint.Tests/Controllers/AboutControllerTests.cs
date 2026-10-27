using Microsoft.AspNetCore.Mvc;
using Waypoint.Controllers;

namespace Waypoint.Tests.Controllers;

public class AboutControllerTests
{
    private readonly AboutController _controller = new();

    [Fact]
    public void Index_ReturnsView() => Assert.IsType<ViewResult>(_controller.Index());

    [Fact]
    public void Kymani_ReturnsView() => Assert.IsType<ViewResult>(_controller.Kymani());

    [Fact]
    public void Riddhi_ReturnsView() => Assert.IsType<ViewResult>(_controller.Riddhi());

    [Fact]
    public void Evan_ReturnsView() => Assert.IsType<ViewResult>(_controller.Evan());
}
