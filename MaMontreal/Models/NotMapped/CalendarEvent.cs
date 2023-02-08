using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MaMontreal.Services;

namespace MaMontreal.Models.NotMapped
{
    public static class CalendarEvent
    {
        //FIXME: Meetings with the same name break the code
        public static string filePath = @"./wwwroot/data/event_data.dat";

        public static void DeleteEventsFile()
        {
            Console.WriteLine("CalanderEvent.cs: Deleting Events Data File");
            if (File.Exists(filePath))
                File.Delete(filePath);
        }

        public static void UpdateEventsFile(IEnumerable<Meeting> meetings)
        {
            Console.WriteLine("CalanderEvent.cs: Updating Events Data File");
            string data = ConvertToString(meetings.ToList<Meeting>());
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

        public static string ConvertToString(Meeting meeting)
        {
            string outStr = "\n\t{"
            ;
            outStr += $"id: {meeting.Id}, " +
                      $"title: '{meeting.EventName}', "
                      ;

            if (meeting.DayOfWeek == null && meeting.Date != null) // non-recurring events
            {
                outStr +=
                $"start: '{String.Format("{0:yyyy-MM-ddTHH:mm:ss}", (meeting.Date.Value.Date + meeting.StartTime.TimeOfDay))}', " +
                $"end: '{String.Format("{0:yyyy-MM-ddTHH:mm:ss}", (meeting.Date.Value.Date + meeting.EndTime.TimeOfDay))}', "
                ;
            }
            else if (meeting.DayOfWeek != null) //recurring events
            {
                outStr +=
                 $"daysOfWeek: '[{(int)meeting.DayOfWeek}]', " +
                 $"startTime: '{String.Format("{0:HH:mm:ss}", meeting.StartTime)}', " +
                 $"endTime: '{String.Format("{0:HH:mm:ss}", meeting.EndTime)}', " +
                 $"startRecur: '{String.Format("{0:yyyy-MM-dd}", meeting.Date == null ? DateTime.Now.Date : meeting.Date.Value.Date)}', "
                 ;
            }

            //TODO: Event color depending on Event Type

            outStr += "},";
            return outStr;
        }

        public static string ConvertToString(List<Meeting> meetingsList)
        {

            string outStr = "[";

            for (int i = 0; i < meetingsList.Count; i++)
            {
                Meeting m = meetingsList[i];
                if (m.DeletedAt != null || (m.DayOfWeek == null && m.Date == null))
                    break;
                outStr += ConvertToString(m);
            }

            outStr += "\n]";
            return outStr;
        }
    }
}