using Microsoft.AspNetCore.Mvc;

namespace Waypoint.Controllers;

[Route("about")]
public class AboutController : Controller
{
    [Route("")]
    public IActionResult Index()
    {
        return View();
    }

    [Route("kymani")]
    public IActionResult Kymani()
    {
        return View();
    }

    [Route("riddhi")]
    public IActionResult Riddhi()
    {
        return View();
    }

    [Route("evan")]
    public IActionResult Evan()
    {
        return View();
    }
}
