using System.Security.Claims;
using MaMontreal.Data;
using MaMontreal.Models;
using MaMontreal.Models.Enums;
using MaMontreal.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MaMontreal.Services
{
    public class MeetingsService : IMeetingsRepo
    {
        private readonly MamDbContext _context;

        ///<exception cref="NullReferenceException"/>
        ///<exception cref="ArgumentException"/>
        public MeetingsService(MamDbContext context)
        {
            if (context == null)
                throw new NullReferenceException("Database context is null!");
            if (context.Meetings == null)
                throw new ArgumentException("Meetings Entity is null!");
            _context = context;
        }

        public bool MeetingExists(int id)
        {
            return (_context.Meetings?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<IEnumerable<Meeting>> GetAllMeetings()
        {
            return await _context.Meetings.ToListAsync<Meeting>();
        }

        ///<exception cref="NullReferenceException"/>
        public async Task<Meeting> GetMeetingById(int? id)
        {
            if (id == null)
                throw new NullReferenceException("Id cannot be null");
            Meeting? meeting = await _context.Meetings.Where(mt => mt.Id == id).FirstOrDefaultAsync();
            if (meeting == null)
                throw new NullReferenceException("No Meeting found with the id " + id);
            return meeting;
        }

        ///<exception cref="ArgumentException"/>
        public async Task<Meeting> CreateMeeting(Meeting meeting, ClaimsPrincipal User)
        {
            if (meeting.DayOfWeek == null && meeting.Date == null)
                throw new ArgumentException("DayOfWeek,Day of Week and Date cannot both be empty!");

            meeting.UpdatedAt = DateTime.Now;
            meeting.UpdatedBy = User.Identity as ApplicationUser;
            meeting.Gsr = User.Identity as ApplicationUser;
            if (User.IsInRole("admin"))
                meeting.Status = Statuses.Approved;

            _context.Meetings.Add(meeting);
            await _context.SaveChangesAsync();
            return meeting;
        }

        ///<exception cref="NullReferenceException"/>
        ///<exception cref="ArgumentException"/>
        public async Task<Meeting> EditMeeting(int? id, Meeting meeting, ClaimsPrincipal User)
        {
            if (id == null)
                throw new NullReferenceException("Id cannot be null");
            // if (_context.Meetings.Find(id.Value) == null)
            //     throw new NullReferenceException("No Meeting Type found with id " + id.Value);
            if (meeting.DayOfWeek == null && meeting.Date == null)
                throw new ArgumentException("DayOfWeek,Day of Week and Date cannot both be empty!");

            meeting.Id = id.Value;

            _context.Meetings.Add(meeting);
            return await EditMeeting(meeting, User);
        }

        ///<exception cref="ArgumentException"/>
        public async Task<Meeting> EditMeeting(Meeting meeting, ClaimsPrincipal User)
        {
            if (meeting.DayOfWeek == null && meeting.Date == null)
                throw new ArgumentException("DayOfWeek,Day of Week and Date cannot both be empty!");

            meeting.UpdatedAt = DateTime.Now;
            meeting.UpdatedBy = User.Identity as ApplicationUser;

            _context.Meetings.Update(meeting);
            await _context.SaveChangesAsync();
            return meeting;
        }

        ///<exception cref="NullReferenceException"/>
        public async Task<Meeting> DeleteMeeting(int? id, ClaimsPrincipal User)
        {
            if (id == null)
                throw new NullReferenceException("Id cannot be null");

            Meeting? meeting = _context.Meetings.Where(mt => mt.Id == id).FirstOrDefault();
            if (meeting == null)
                throw new NullReferenceException("No meeting type found with the id provided");

            return await DeleteMeeting(meeting, User);
        }

        public async Task<Meeting> DeleteMeeting(Meeting meeting, ClaimsPrincipal User)
        {
            meeting.UpdatedAt = DateTime.Now;
            meeting.UpdatedBy = User.Identity as ApplicationUser;
            meeting.DeletedAt = DateTime.Now;

            _context.Meetings.Update(meeting);
            await _context.SaveChangesAsync();
            return meeting;
        }

        ///<exception cref="NullReferenceException"/>
        public async Task<Meeting> UnDeleteMeeting(int? id, ClaimsPrincipal User)
        {
            if (id == null)
                throw new NullReferenceException("Id cannot be null");

            Meeting? meeting = _context.Meetings.Where(mt => mt.Id == id).FirstOrDefault();
            if (meeting == null)
                throw new NullReferenceException("No meeting type found with the id provided");

            return await UnDeleteMeeting(meeting, User);
        }

        public async Task<Meeting> UnDeleteMeeting(Meeting meeting, ClaimsPrincipal User)
        {
            meeting.UpdatedAt = DateTime.Now;
            meeting.UpdatedBy = User.Identity as ApplicationUser;
            meeting.DeletedAt = null;

            _context.Meetings.Update(meeting);
            await _context.SaveChangesAsync();
            return meeting;
        }
    }
}