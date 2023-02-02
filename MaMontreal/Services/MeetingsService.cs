using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MaMontreal.Data;
using MaMontreal.Models;
using MaMontreal.Repositories;

namespace MaMontreal.Services
{
    public class MeetingsService : IMeetingsRepo
    {
        private readonly MamDbContext _context;

        public MeetingsService(MamDbContext context)
        {
            _context = context;
        }

        public Meeting CreateMeeting(Meeting meeting)
        {
            meeting.Id = 1;
            Console.WriteLine("New meeting created " + meeting);

            return meeting;
        }

        public Meeting DeleteMeeting(Meeting meeting)
        {
            throw new NotImplementedException();
        }

        public Meeting DeleteMeetingById(int id)
        {
            throw new NotImplementedException();
        }

        public Meeting EditMeeting(Meeting meeting)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Meeting> GetAllMeetings()
        {
            throw new NotImplementedException();
        }

        public Meeting GetMeetingById(int id)
        {
            throw new NotImplementedException();
        }
    }
}