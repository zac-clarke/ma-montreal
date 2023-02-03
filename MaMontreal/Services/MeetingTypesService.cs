using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MaMontreal.Data;
using MaMontreal.Models;
using MaMontreal.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MaMontreal.Services
{
    public class MeetingTypesService : IMeetingTypesRepo
    {
        private readonly MamDbContext _context;

        public MeetingTypesService(MamDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MeetingType>> GetAllMeetingTypes()
        {
            return await _context.MeetingTypes.ToListAsync<MeetingType>();
        }

        public async Task<MeetingType?> GetMeetingTypeById(int id)
        {
            return await _context.MeetingTypes.Where(mt => mt.Id == id).FirstOrDefaultAsync();
            // return _context.MeetingTypes.Where(mt => mt.Id == id).FirstOrDefault();
        }

        public async Task<MeetingType> CreateMeetingType(MeetingType meetingType)
        {
            _context.MeetingTypes.Add(meetingType);
            await _context.SaveChangesAsync();
            return meetingType;
        }

        public async Task<MeetingType> UpdateMeetingType(int id, MeetingType meetingType)
        {
            MeetingType? oldMeetingType = _context.MeetingTypes.Where(mt => mt.Id == id).FirstOrDefault();
            if (oldMeetingType == null)
                throw new KeyNotFoundException("No meeting type found with the id provided");
            meetingType.Id = oldMeetingType.Id;
            return await UpdateMeetingType(meetingType);
        }

        public async Task<MeetingType> UpdateMeetingType(MeetingType meetingType)
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