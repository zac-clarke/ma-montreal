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
    public class LanguagesService
    {
        private readonly MamDbContext _context;


        public LanguagesService(MamDbContext context)
        {
            if (context == null)
                throw new NullReferenceException("Database context is null!");
            if (context.Languages == null)
                throw new NullReferenceException("No Languages table in database!");
            _context = context;
        }


        public async Task<List<Language>> GetAllAsync()
        {
            return await _context.Languages.ToListAsync();
        }

        public async Task<Language> GetAsync(int? id)
        {
            if (id == null)
            {
                throw new NullReferenceException("Parameter 'id' is null.");
            }
            var language = await _context.Languages.FindAsync(id);
            if (language == null)
            {
                throw new NullReferenceException("Language not found.");
            }
            return language;
        }

        public async Task CreateAsync(Language language)
        {
            _context.Add(language);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int? id, Language language)
        {
            if (id != language.Id)
            {
                throw new NullReferenceException("Language not found.");
            }
            _context.Update(language);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int? id)
        {
            if (id == null)
            {
                throw new NullReferenceException("Parameter 'id' is null.");
            }
            var language = await this.GetAsync(id);
            _context.Languages.Remove(language);
            await _context.SaveChangesAsync();
        }
    }
}