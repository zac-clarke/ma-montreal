using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MaMontreal.Data;
using MaMontreal.Models;
using MaMontreal.Repositories;

namespace MaMontreal.Services
{
    public class MeetingTypesService : IMeetingTypesRepo
    {
        private readonly MamDbContext _context;

        public MeetingTypesService(MamDbContext context)
        {
            _context = context;
        }

        public IEnumerable<MeetingType> GetAllMeetingTypes()
        {
            return _context.MeetingTypes.AsEnumerable<MeetingType>();
        }

        public MeetingType? GetMeetingTypeById(int id)
        {
            return _context.MeetingTypes.Where(mt => mt.Id == id).FirstOrDefault();
        }

        public async Task<MeetingType> CreateMeetingType(MeetingType meetingType)
        {
            _context.MeetingTypes.Add(meetingType);
            await _context.SaveChangesAsync();
            return meetingType;
        }

        public async Task<MeetingType> EditMeetingType(int id, MeetingType meetingType)
        {
            MeetingType? oldMeetingType = _context.MeetingTypes.Where(mt => mt.Id == id).FirstOrDefault();
            if (oldMeetingType == null)
                throw new KeyNotFoundException("No meeting type found with the id provided");
            meetingType.Id = oldMeetingType.Id;
            return await EditMeetingType(meetingType);
        }

        public async Task<MeetingType> EditMeetingType(MeetingType meetingType)
        {
            _context.MeetingTypes.Update(meetingType);
            await _context.SaveChangesAsync();
            return meetingType;
        }

        public async Task<MeetingType> DeleteMeetingType(int id)
        {
            MeetingType? meetingType = _context.MeetingTypes.Where(mt => mt.Id == id).FirstOrDefault();
            if (meetingType == null)
                throw new KeyNotFoundException("No meeting type found with the id provided");
            return await DeleteMeetingType(meetingType);
        }

        public async Task<MeetingType> DeleteMeetingType(MeetingType meetingType)
        {
            _context.MeetingTypes.Remove(meetingType);
            await _context.SaveChangesAsync();
            return meetingType;
        }
    }
}