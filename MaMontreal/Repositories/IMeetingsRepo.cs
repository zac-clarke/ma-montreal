using System.Security.Claims;
using MaMontreal.Models;

namespace MaMontreal.Repositories
{
    public interface IMeetingsRepo
    {
        public bool MeetingExists(int id);
        public Task<IEnumerable<Meeting>> GetAllMeetings();
        public Task<Meeting> GetMeetingById(int? id);
        public Task<Meeting> CreateMeeting(Meeting meeting, ClaimsPrincipal User);
        public Task<Meeting> EditMeeting(int? id, Meeting meeting, ClaimsPrincipal User);
        public Task<Meeting> EditMeeting(Meeting meeting, ClaimsPrincipal User);
        public Task<Meeting> DeleteMeeting(int? id, ClaimsPrincipal User);
        public Task<Meeting> DeleteMeeting(Meeting meeting, ClaimsPrincipal User);
        public Task<Meeting> UnDeleteMeeting(int? id, ClaimsPrincipal User);
        public Task<Meeting> UnDeleteMeeting(Meeting meeting, ClaimsPrincipal User);
    }
}