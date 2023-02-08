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

    private readonly MeetingsService _meetingService = null!;

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

    [Route("Calendar")]
    public IActionResult Calendar()
    {
        ViewData["meetings"] = CalendarEvent.GetEventStringFromFile(_meetingService);
        return View();
    }

    [Route("Calendar")]
    [HttpDelete, ActionName("Calendar")]
    public IActionResult CalendarReset()
    {
        CalendarEvent.DeleteEventsFile();
        ViewData["meetings"] = CalendarEvent.GetEventStringFromFile(_meetingService);
        return View();
    }

    [Route("Error")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
