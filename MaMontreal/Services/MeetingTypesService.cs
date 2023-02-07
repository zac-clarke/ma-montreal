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

        ///<exception cref="NullReferenceException"/>
        ///<exception cref="ArgumentException"/>
        public MeetingTypesService(MamDbContext context)
        {
            if (context == null)
                throw new NullReferenceException("Database context is null!");
            if (context.MeetingTypes == null)
                throw new ArgumentException("MeetingTypes Entity is null!");
            _context = context;
        }

        public bool MeetingTypeExists(int id)
        {
            return (_context.MeetingTypes?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<IEnumerable<MeetingType>> GetAllMeetingTypes()
        {
            return await _context.MeetingTypes.ToListAsync<MeetingType>();
        }

        ///<exception cref="NullReferenceException"/>
        public async Task<MeetingType> GetMeetingTypeById(int? id)
        {
            if (id == null)
                throw new NullReferenceException("Id cannot be null");
            MeetingType? meetingType = await _context.MeetingTypes.Where(mt => mt.Id == id).FirstOrDefaultAsync();
            if (meetingType == null)
                throw new NullReferenceException("No MeetingType found with the id " + id);
            return meetingType;
        }

        public async Task<MeetingType> CreateMeetingType(MeetingType meetingType)
        {
            _context.MeetingTypes.Add(meetingType);
            await _context.SaveChangesAsync();
            return meetingType;
        }

        ///<exception cref="NullReferenceException"/>
        public async Task<MeetingType> EditMeetingType(int? id, MeetingType meetingType)
        {
            if (id == null)
                throw new NullReferenceException("Id cannot be null");
            // if (_context.MeetingTypes.Find(id.Value) == null)
            //     throw new NullReferenceException("No Meeting Type found with id " + id.Value);

            meetingType.Id = id.Value;
            return await EditMeetingType(meetingType);
        }

        public async Task<MeetingType> EditMeetingType(MeetingType meetingType)
        {
            _context.MeetingTypes.Update(meetingType);
            await _context.SaveChangesAsync();
            return meetingType;
        }

        ///<exception cref="NullReferenceException"/>
        public async Task<MeetingType> DeleteMeetingType(int? id)
        {
            if (id == null)
                throw new NullReferenceException("Id cannot be null");

            MeetingType? meetingType = _context.MeetingTypes.Where(mt => mt.Id == id).FirstOrDefault();
            if (meetingType == null)
                throw new NullReferenceException("No meeting type found with the id provided");

            return await DeleteMeetingType(meetingType);
        }

        ///<exception cref="DbUpdateException"/>
        public async Task<MeetingType> DeleteMeetingType(MeetingType meetingType)
        {
            int numMeetingsWithThisType = _context.Meetings.Where(m => m.MeetingType.Id == meetingType.Id).Count();
            if (numMeetingsWithThisType > 0)
                throw new DbUpdateException("Cannot delete this Meeting Type. There are meetings of this Type!");

            _context.MeetingTypes.Remove(meetingType);
            await _context.SaveChangesAsync();
            return meetingType;
        }
    }
}