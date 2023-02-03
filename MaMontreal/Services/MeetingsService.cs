using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MaMontreal.Data;
using MaMontreal.Models;
using MaMontreal.Repositories;
using Microsoft.AspNetCore.Identity;
namespace MaMontreal.Services
{
    public class MeetingsService : IMeetingsRepo
    {
        private readonly MamDbContext _context;

        public MeetingsService(MamDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Meeting>> GetAllMeetings()
        {
            throw new NotImplementedException();
        }

        public async Task<Meeting> GetMeetingById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Meeting> CreateMeeting(Meeting meeting, ApplicationUser user)
        {
            meeting.Gsr = user;
            meeting.UpdatedBy = user;
            meeting.CreatedAt = DateTime.Now;
            meeting.UpdatedAt = DateTime.Now;
            meeting.Status = Status.Pending;
            _context.Meetings.Add(meeting);
            await _context.SaveChangesAsync();
            return meeting;
        }

        public async Task<Meeting> DeleteMeeting(Meeting meeting)
        {
            throw new NotImplementedException();
        }

        public async Task<Meeting> DeleteMeetingById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Meeting> EditMeeting(Meeting meeting)
        {
            throw new NotImplementedException();
        }
    }
}