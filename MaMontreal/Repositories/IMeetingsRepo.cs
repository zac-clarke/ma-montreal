using System.Security.Claims;
using MaMontreal.Models;
using Microsoft.AspNetCore.Identity;

namespace MaMontreal.Repositories
{
    public interface IMeetingsRepo
    {
        public bool MeetingExists(int id);
        public Task<IEnumerable<Meeting>> GetAllMeetings();
        public Task<Meeting> GetMeetingById(int? id);
        public Task<Meeting> CreateMeeting(Meeting meeting, UserManager<ApplicationUser> userManager, ClaimsPrincipal User);
        public Task<Meeting> EditMeeting(int? id, Meeting meeting, UserManager<ApplicationUser> userManager, ClaimsPrincipal User);
        public Task<Meeting> EditMeeting(Meeting meeting, UserManager<ApplicationUser> userManager, ClaimsPrincipal User);
        public Task<Meeting> DeleteMeeting(int? id, UserManager<ApplicationUser> userManager, ClaimsPrincipal User);
        public Task<Meeting> DeleteMeeting(Meeting meeting, UserManager<ApplicationUser> userManager, ClaimsPrincipal User);
        public Task<Meeting> UnDeleteMeeting(int? id, UserManager<ApplicationUser> userManager, ClaimsPrincipal User);
        public Task<Meeting> UnDeleteMeeting(Meeting meeting, UserManager<ApplicationUser> userManager, ClaimsPrincipal User);
    }
}