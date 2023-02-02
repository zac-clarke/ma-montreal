using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MaMontreal.Models;

namespace MaMontreal.Repositories
{
    public interface IMeetingsRepo
    {
        public IEnumerable<Meeting> GetAllMeetings();
        public Meeting GetMeetingById(int id);
        public Meeting CreateMeeting(Meeting meeting);
        public Meeting EditMeeting(Meeting meeting);
        public Meeting DeleteMeetingById(int id);
        public Meeting DeleteMeeting(Meeting meeting);

    }
}