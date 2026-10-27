using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Waypoint.Models;

namespace Waypoint.Controllers;

public class HomeController : Controller
{
    [Route("")]
    public IActionResult Index()
    {
        return View();
    }

    [Route("privacy")]
    public IActionResult Privacy()
    {
        return View();
    }

    [Route("error")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
