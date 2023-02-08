
using MaMontreal.Data;
using MaMontreal.Models;
using Microsoft.EntityFrameworkCore;

namespace MaMontreal.Services
{
    public class TagsService
    {
        private readonly MamDbContext _context;


        public TagsService(MamDbContext context)
        {
            if (context == null)
                throw new NullReferenceException("Database context is null!");
            if (context.Tags == null)
                throw new NullReferenceException("No Tags table in database!");
            _context = context;
        }


        public async Task<List<Tag>> GetAllAsync()
        {
            return await _context.Tags.ToListAsync();
        }

        public async Task<Tag> GetAsync(int? id)
        {
            if (id == null)
            {
                throw new NullReferenceException("Parameter 'id' is null.");
            }
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null)
            {
                throw new NullReferenceException("Tag not found.");
            }
            return tag;
        }

        public async Task CreateAsync(Tag tag)
        {
            _context.Add(tag);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int? id, Tag tag)
        {
            if (id != tag.Id)
            {
                throw new NullReferenceException("Tag not found.");
            }
            _context.Update(tag);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int? id)
        {
            if (id == null)
            {
                throw new NullReferenceException("Parameter 'id' is null.");
            }
            var tag = await this.GetAsync(id);
            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
        }
    }
}