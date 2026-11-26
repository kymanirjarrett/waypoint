using Microsoft.AspNetCore.Mvc;

namespace Waypoint.Controllers;

[Route("about")]
public class AboutController : Controller
{
    [Route("")]
    public IActionResult Index() => View();

    [Route("kymani")]
    public IActionResult Kymani() => View();

    [Route("riddhi")]
    public IActionResult Riddhi() => View();

    [Route("evan")]
    public IActionResult Evan() => View();
}
