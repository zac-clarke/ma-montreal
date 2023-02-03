using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MaMontreal.Models;

namespace MaMontreal.Repositories
{
    public interface IMeetingTypesRepo
    {
        public Task<IEnumerable<MeetingType>> GetAllMeetingTypes();
        public Task<MeetingType?> GetMeetingTypeById(int id);
        public Task<MeetingType> CreateMeetingType(MeetingType meetingType);
        public Task<MeetingType> UpdateMeetingType(int id, MeetingType meetingType);
        public Task<MeetingType> UpdateMeetingType(MeetingType meetingType);
        public Task<MeetingType> DeleteMeetingType(int id);
        public Task<MeetingType> DeleteMeetingType(MeetingType meetingType);
    }
}