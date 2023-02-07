using System.Diagnostics;
using MaMontreal.Data;
using MaMontreal.Models;
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
        IEnumerable<Meeting> meetings = _meetingService.GetAllMeetings(m => m.Id);
        List<CalendarEvent> meetingList = new List<CalendarEvent>();

        DateTime today = DateTime.Now.Date;

        foreach (Meeting m in meetings)
        {
            CalendarEvent? e = null;

            if (m.DeletedAt != null || (m.DayOfWeek == null && m.Date == null))
            {
                break;
            }
            else if (m.DayOfWeek == null && m.Date != null)
            {
                e = new CalendarEvent(m.Id, m.EventName, m.Date.Value, m.Date.Value, m.StartTime, m.EndTime);
                meetingList.Add(e);
            }
            else if (m.DayOfWeek != null)
            {
                DateTime start = m.Date == null ? m.CreatedAt.Value : m.Date.Value;
                int targetDay = (int)m.DayOfWeek;
                if (targetDay <= (int)start.DayOfWeek)
                    targetDay += 7;
                start = start.AddDays((targetDay - (int)start.DayOfWeek) % 7);

                DateTime end = today.AddYears(1);
                double numWeeks = ((end - start).TotalDays) / 7 + 1;

                for (int i = 0; i < numWeeks; i++)
                {
                    DateTime date = start.Date.AddDays(i * 7);
                    e = new CalendarEvent(m.Id, m.EventName, date, date, m.StartTime, m.EndTime);
                    meetingList.Add(e);
                }
            }
        }
        ViewData["meetings"] = CalendarEvent.ToJson(meetingList);
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

    public class CalendarEvent
    {
        private int Id { get; set; }
        private string Title { get; set; }
        private string Start { get; set; }
        private string End { get; set; }

        public CalendarEvent(int id, String title, DateTime startDate, DateTime endDate, DateTime startTime, DateTime endTime)
        {
            Id = id;
            Title = title;
            Start = String.Format("{0:yyyy-MM-ddTHH:mm:ss}", (startDate.Date + startTime.TimeOfDay));
            End = String.Format("{0:yyyy-MM-ddTHH:mm:ss}", (endDate.Date + endTime.TimeOfDay));
        }

        public static String ToJson(CalendarEvent e)
        {
            return "{" +
                "id: " + $"'{e.Id}'," +
                "title: " + $"'{e.Title}'," +
                "start: " + $"'{e.Start}'," +
                "end: " + $"'{e.End}'" +
            "}"
            ;
        }

        public static String ToJson(List<CalendarEvent> list)
        {
            string outStr = "[";
            for (int i = 0; i < list.Count; i++)
            {
                outStr += ToJson(list[i]);
                if (i < list.Count - 1)
                    outStr += ",";
            }
            outStr += "]";
            return outStr;
        }

    }
}
