using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MaMontreal.Data;
using MaMontreal.Models;

namespace MaMontreal.Repositories
{
    public class LanguagesRepo : IRepo<Language>
    {
        private MamDbContext _context;

        public LanguagesRepo(MamDbContext context)
        {
            _context = context;
        }
        public async Task<Language> Get(int id)
        {
            if (_context.Languages != null)
                throw new SystemException("Entity set 'MamDbContext.Languages' is null.");
            //  return await _context.Languages.Where(l=> l.Id == id && l.DeletedAt != null).FirstOrDefaultAsync();
            return await _context.Languages.FindAsync(id);
        }

        public Task<IEnumerable<Language>> GetAll()
        {
            throw new NotImplementedException();
        }
        public Task<Language> Add(Language entity)
        {
            throw new NotImplementedException();
        }

        public Task<Language> Update(Language entity)
        {
            throw new NotImplementedException();
        }

        public Task<Language> Delete(int entity)
        {
            throw new NotImplementedException();
        }

        public Task<Language> Delete(Language entity)
        {
            throw new NotImplementedException();
        }

        public Task<Language> GetDeleted(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Language> GetAllDeleted(string id)
        {
            throw new NotImplementedException();
        }
    }
}