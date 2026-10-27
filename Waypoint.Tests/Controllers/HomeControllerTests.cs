using Microsoft.AspNetCore.Mvc;
using Waypoint.Controllers;

namespace Waypoint.Tests.Controllers;

public class HomeControllerTests
{
    private readonly HomeController _controller = new();

    [Fact]
    public void Index_ReturnsView() => Assert.IsType<ViewResult>(_controller.Index());

    [Fact]
    public void Privacy_ReturnsView() => Assert.IsType<ViewResult>(_controller.Privacy());
}
