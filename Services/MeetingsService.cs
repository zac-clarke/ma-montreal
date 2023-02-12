using System.Security.Claims;
using MaMontreal.Data;
using MaMontreal.Models;
using MaMontreal.Models.Enums;
using MaMontreal.Models.NotMapped;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MaMontreal.Services
{
    public class MeetingsService
    {
        private readonly MamDbContext _context;
        private UserManager<ApplicationUser> _userManager = null!;

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
            return await _context.Meetings
                                .Include(m => m.Gsr)
                                .Include(m => m.UpdatedBy)
                                .Include(m => m.Language)
                                .Include(m => m.MeetingType)
                                .ToListAsync<Meeting>();
        }

        public IEnumerable<Meeting> GetAllMeetings(Func<Meeting, object> orderBy)
        {
            return _context.Meetings
                                .Include(m => m.Gsr)
                                .Include(m => m.UpdatedBy)
                                .Include(m => m.Language)
                                .Include(m => m.MeetingType)
                                .OrderBy(orderBy)
                                .ToList<Meeting>();
        }

        ///<exception cref="NullReferenceException"/>
        public async Task<Meeting> GetMeetingById(int? id)
        {
            if (id == null)
                throw new NullReferenceException("Id cannot be null");
            Meeting? meeting = await _context.Meetings
                                            .Where(mt => mt.Id == id)
                                            .Include(m => m.Gsr)
                                            .Include(m => m.UpdatedBy)
                                            .Include(m => m.Language)
                                            .Include(m => m.MeetingType)
                                            .FirstOrDefaultAsync();
            if (meeting == null)
                throw new NullReferenceException("No Meeting found with the id " + id);
            return meeting;
        }

        ///<exception cref="ArgumentException"/>
        public async Task<Meeting> CreateMeeting(Meeting meeting, UserManager<ApplicationUser> userManager, ClaimsPrincipal User, AzureStorageService azureStorage)
        {
            if (meeting._ImageFile != null && meeting._ImageFile.Length > 0)
            {
                IFormFile file = meeting._ImageFile;
                BlobResponseDto? response = await azureStorage.UploadMeetingImage(file, meeting);
                if (!response.Error)
                {
                    meeting.ImageUrl = response.Blob.Uri;
                }
            }

            ApplicationUser? curUser = userManager?.GetUserAsync(User)?.Result;
            MeetingType? meetingType = _context.MeetingTypes?.Where(mt => mt.Id == meeting._MeetingTypeId)?.FirstOrDefault();
            Language? language = _context.Languages?.Where(l => l.Id == meeting._LanguageId)?.FirstOrDefault();

            if (meeting.DayOfWeek == null && meeting.Date == null)
                throw new ArgumentException("Date,Date and Recurring Day cannot both be empty!");
            else if (curUser == null)
                throw new ArgumentException("Current User is invalid!");
            else if (meetingType == null)
                throw new ArgumentException("Meeting Type is invalid!");
            else if (language == null)
                throw new ArgumentException("Language is invalid!");
            else if (meeting._GsrAssignedId == null)
                throw new ArgumentException("_GsrAssignedId,Gsr is invalid!");

            meeting.Gsr = _context.Users.Where(u => u.Id == meeting._GsrAssignedId).FirstOrDefault();
            if (meeting.Gsr == null)
                throw new ArgumentException("_GsrAssignedId,Gsr is invalid!");

            if (User.IsInRole("gsr"))
                meeting.Status = Statuses.Pending;
            meeting.PostalCode = meeting.PostalCode
                .Replace(" ", string.Empty)
                .Replace("-", string.Empty)
                .ToUpper();
            meeting.Language = language;
            meeting.MeetingType = meetingType;
            meeting.UpdatedAt = DateTime.Now;
            meeting.UpdatedBy = curUser;

            if (User.IsInRole("admin"))
                meeting.Status = Statuses.Approved;

            _context.Meetings.Add(meeting);
            await _context.SaveChangesAsync();

            //Update data file
            CalendarEvent.UpdateEventsFile(_context.Meetings.ToList<Meeting>());

            return meeting;
        }

        ///<exception cref="NullReferenceException"/>
        ///<exception cref="ArgumentException"/>
        public async Task<Meeting> EditMeeting(int? id, Meeting meeting, UserManager<ApplicationUser> userManager, ClaimsPrincipal User, AzureStorageService azureStorage)
        {
            if (meeting._ImageFile != null && meeting._ImageFile.Length > 0)
            {
                IFormFile file = meeting._ImageFile;
                BlobResponseDto? response = await azureStorage.UploadMeetingImage(file, meeting);
                if (!response.Error)
                {
                    meeting.ImageUrl = response.Blob.Uri;
                }
                else
                {
                    // TODO:
                    throw new ArgumentException(message: $"_ImageFile, {response.Status}");
                }
            }
            if (id == null)
                throw new NullReferenceException("Id,Id cannot be null");
            // if (_context.Meetings.Find(id.Value) == null)
            //     throw new NullReferenceException("No Meeting Type found with id " + id.Value);

            meeting.Id = id.Value;

            // _context.Meetings.Add(meeting);
            return await EditMeeting(meeting, userManager, User);
        }

        ///<exception cref="ArgumentException"/>
        public async Task<Meeting> EditMeeting(Meeting meeting, UserManager<ApplicationUser> userManager, ClaimsPrincipal User)
        {
            ApplicationUser? curUser = userManager?.GetUserAsync(User)?.Result;
            MeetingType? meetingType = _context.MeetingTypes?.Where(mt => mt.Id == meeting._MeetingTypeId)?.FirstOrDefault();
            Language? language = _context.Languages?.Where(l => l.Id == meeting._LanguageId)?.FirstOrDefault();


            if (meeting.DayOfWeek == null && meeting.Date == null)
                throw new ArgumentException("Date,Date and Recurring Day cannot both be empty!");
            else if (curUser == null)
                throw new ArgumentException("Id,Current User is invalid!");
            else if (meetingType == null)
                throw new ArgumentException("_MeetingTypeId,Meeting Type is invalid!");
            else if (language == null)
                throw new ArgumentException("_LanguageId,Language is invalid!");
            else if (meeting._GsrAssignedId == null)
                throw new ArgumentException("_GsrAssignedId,Gsr is invalid!");

            meeting.Gsr = _context.Users.Where(u => u.Id == meeting._GsrAssignedId).FirstOrDefault();
            if (meeting.Gsr == null)
                throw new ArgumentException("_GsrAssignedId,Gsr is invalid!");

            meeting.PostalCode = meeting.PostalCode
                .Replace(" ", string.Empty)
                .Replace("-", string.Empty)
                .ToUpper();
            meeting.MeetingType = meetingType;
            meeting.Language = language;
            meeting.UpdatedAt = DateTime.Now;
            meeting.UpdatedBy = curUser;

            _context.Meetings.Update(meeting);
            await _context.SaveChangesAsync();


            //Update data file
            CalendarEvent.UpdateEventsFile(_context.Meetings.ToList<Meeting>());

            return meeting;
        }

        ///<exception cref="NullReferenceException"/>
        public async Task<Meeting> DeleteMeeting(int? id, UserManager<ApplicationUser> userManager, ClaimsPrincipal User)
        {
            if (id == null)
                throw new NullReferenceException("Id cannot be null");

            Meeting? meeting = _context.Meetings.Where(mt => mt.Id == id).FirstOrDefault();
            if (meeting == null)
                throw new NullReferenceException("No meeting type found with the id provided");

            return await DeleteMeeting(meeting, userManager, User);
        }

        public async Task<Meeting> DeleteMeeting(Meeting meeting, UserManager<ApplicationUser> userManager, ClaimsPrincipal User)
        {
            ApplicationUser? curUser = await userManager.GetUserAsync(User);
            if (curUser == null)
                throw new NullReferenceException("User cannot be null");
            meeting.UpdatedAt = DateTime.Now;
            meeting.UpdatedBy = curUser;
            meeting.DeletedAt = DateTime.Now;

            _context.Meetings.Update(meeting);
            await _context.SaveChangesAsync();

            //Update data file
            CalendarEvent.UpdateEventsFile(_context.Meetings.ToList<Meeting>());

            return meeting;
        }

        ///<exception cref="NullReferenceException"/>
        public async Task<Meeting> RestoreMeeting(int? id, UserManager<ApplicationUser> userManager, ClaimsPrincipal User)
        {
            if (id == null)
                throw new NullReferenceException("Id cannot be null");

            Meeting? meeting = _context.Meetings.Where(mt => mt.Id == id).FirstOrDefault();
            if (meeting == null)
                throw new NullReferenceException("No meeting type found with the id provided");

            return await RestoreMeeting(meeting, userManager, User);
        }

        public async Task<Meeting> RestoreMeeting(Meeting meeting, UserManager<ApplicationUser> userManager, ClaimsPrincipal User)
        {
            ApplicationUser? curUser = await userManager.GetUserAsync(User);
            if (curUser == null)
                throw new NullReferenceException("User cannot be null");
            meeting.UpdatedAt = DateTime.Now;
            meeting.UpdatedBy = curUser;
            meeting.DeletedAt = null;

            _context.Meetings.Update(meeting);
            await _context.SaveChangesAsync();

            //Update data file
            CalendarEvent.UpdateEventsFile(_context.Meetings.ToList<Meeting>());

            return meeting;
        }

        public async Task<IEnumerable<Meeting>> GetAllMeetingsByGsrId(string? userId)
        {
            if (userId == null)
                throw new NullReferenceException("Trying to Fetch User meetings failed. UserId is null");
            return await _context.Meetings
                                    .Include(m => m.Gsr)
                                    .Include(m => m.UpdatedBy)
                                    .Include(m => m.Language)
                                    .Include(m => m.MeetingType)
                                    .Where(m => m.Gsr != null && m.Gsr.Id == userId)
                                    .ToListAsync<Meeting>();
        }

        public async Task<bool> GetAnyPendingAsync()
        {
            var pendingMeetingList = await _context.Meetings.Where(x => x.Status == Statuses.Pending).ToListAsync();
            if (pendingMeetingList.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        internal async Task HandleAsync(int? id, Statuses status, ClaimsPrincipal User, UserManager<ApplicationUser> userManager)
        {
            Meeting? meeting = await _context.Meetings.Where(r => r.Id == id).FirstOrDefaultAsync();
            if (meeting == null)
                throw new NullReferenceException("No meeting Found");

            ApplicationUser? reqHandler = await userManager.GetUserAsync(User);
            if (reqHandler == null || userManager.IsInRoleAsync(reqHandler, "admin").Result == false)
                throw new NullReferenceException("You don't have permission to approve this Meeting");
            meeting.Status = status;
            meeting.UpdatedAt = DateTime.Now;
            meeting.UpdatedBy = reqHandler;

            _context.Update(meeting);
            await _context.SaveChangesAsync();
        }
    }
}