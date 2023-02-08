using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MaMontreal.Services;

namespace MaMontreal.Models.NotMapped
{
    public class CalendarEvent
    {
        public static string filePath = @"./wwwroot/data/event_data.dat";
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

        public static void DeleteEventsFile()
        {
            Console.WriteLine("CalanderEvent.cs: Deleting Events Data File");
            // IEnumerable<Meeting> meetings = _meetingService.GetAllMeetings(m => m.Id);
            if (File.Exists(filePath))
                File.Delete(filePath);
        }

        public static void UpdateEventsFile(IEnumerable<Meeting> meetings)
        {
            Console.WriteLine("CalanderEvent.cs: Updating Events Data File");
            // IEnumerable<Meeting> meetings = _meetingService.GetAllMeetings(m => m.Id);
            string data = ConvertMeetingsToEventString(meetings);
            (new FileInfo(filePath)).Directory?.Create();
            File.WriteAllText(filePath, data);
        }

        public static string GetEventStringFromFile(MeetingsService _meetingService)
        {
            //Update if file doesn't exist or if it's too old
            if (!File.Exists(filePath) || (DateTime.Now - File.GetLastWriteTime(filePath)).TotalDays > 7)
            {
                IEnumerable<Meeting> meetings = _meetingService.GetAllMeetings().Result;
                if (meetings == null)
                    meetings = new List<Meeting>();
                UpdateEventsFile(meetings);
            }
            return File.ReadAllText(filePath);
        }

        public static string ConvertMeetingsToEventString(IEnumerable<Meeting> meetings)
        {
            List<CalendarEvent> eventsList = new List<CalendarEvent>();

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
                    eventsList.Add(e);
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
                        eventsList.Add(e);
                    }
                }
            }

            return ToJson(eventsList);
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