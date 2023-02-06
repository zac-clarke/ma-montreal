using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MaMontreal.Models;

namespace MaMontreal.Controllers;

[Route("/Meetings")]
public class MeetingsController : Controller
{


    [Route("Calandar")]
    public IActionResult Calandar()
    {
        return View();
    }

    [Route("MeetingsList")]
    public IActionResult MeetingsList()
    {
        return View();
    }

    [Route("Error")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
