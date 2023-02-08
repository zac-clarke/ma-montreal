using System.Diagnostics;
using MaMontreal.Data;
using MaMontreal.Models;
using MaMontreal.Models.NotMapped;
using MaMontreal.Services;
using Microsoft.AspNetCore.Mvc;

namespace MaMontreal.Controllers;

[Route("/Meetings")]
public class MeetingsController : Controller
{

    private readonly MeetingsService _meetingService;

    public MeetingsController(MamDbContext context)
    {
        try
        {
            _meetingService = new MeetingsService(context);
        }
        catch (SystemException ex)
        {
            Problem(ex.Message);
        }
    }

    [Route("Calandar")]
    public IActionResult Calandar()
    {
        ViewData["meetings"] = CalendarEvent.GetEventStringFromFile(_meetingService);
        return View();
    }

    [Route("Calandar")]
    [HttpDelete, ActionName("Calendar")]
    public IActionResult CalandarReset()
    {
        CalendarEvent.DeleteEventsFile();
        ViewData["meetings"] = CalendarEvent.GetEventStringFromFile(_meetingService);
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
