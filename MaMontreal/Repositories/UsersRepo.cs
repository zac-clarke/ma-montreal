using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MaMontreal.Data;
using MaMontreal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MaMontreal.Repositories
{
    public class UsersRepo
    {

        private MamDbContext _context;
        public UserManager<ApplicationUser> _userManagaer { get; set; }

        public UsersRepo(MamDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManagaer = userManager;

        }

        public async Task<ApplicationUser> Get(string id)
        {
            if (_context.Users != null)
                throw new SystemException("Entity set 'MamDbContext.Users' is null.");
            return await _userManagaer.FindByIdAsync(id);
        }

        public async Task<IEnumerable<ApplicationUser>> GetAll()
        {
            if (_context.Users != null)
                throw new SystemException("Entity set 'MamDbContext.Users' is null.");
            return await _context.Users.ToListAsync();
        }
        public Task<ApplicationUser> Add(ApplicationUser entity)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser> Delete(ApplicationUser entity)
        {
            throw new NotImplementedException();
        }



        public Task<ApplicationUser> GetAllDeleted(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser> GetDeleted(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser> Update(ApplicationUser entity)
        {
            throw new NotImplementedException();
        }
    }
}