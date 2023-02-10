using System.Diagnostics;
using MaMontreal.Data;
using MaMontreal.Models;
using MaMontreal.Models.NotMapped;
using MaMontreal.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MaMontreal.Controllers;

[Route("/Meetings")]
public class MeetingsController : Controller
{

    private readonly MeetingsService _meetingService = null!;
    private readonly ILogger<MeetingsController> _logger = null!;

    public MeetingsController(MamDbContext context, ILogger<MeetingsController> logger)
    {
        try
        {
            _meetingService = new MeetingsService(context);
            _logger = logger;
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

    [Route("MeetingDetails")]
    public async Task<IActionResult> MeetingDetails(int? id)
    {
        try
        {
            Meeting meeting = await _meetingService.GetMeetingById(id);
            return View(meeting);
        }
        catch (NullReferenceException ex)
        {
            TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage(ex.Message, "danger"));
            _logger.LogError(ex.Message);
            return RedirectToAction(nameof(Index));
        }
    }

    [Route("Error")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
