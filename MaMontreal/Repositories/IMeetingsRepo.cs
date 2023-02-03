using MaMontreal.Models;

namespace MaMontreal.Repositories
{
    public interface IMeetingsRepo
    {
        public Task<IEnumerable<Meeting>> GetAllMeetings();
        public Task<Meeting> GetMeetingById(int id);
        public Task<Meeting> CreateMeeting(Meeting meeting, ApplicationUser User);
        public Task<Meeting> EditMeeting(Meeting meeting);
        public Task<Meeting> DeleteMeetingById(int id);
        public Task<Meeting> DeleteMeeting(Meeting meeting);

    }
}