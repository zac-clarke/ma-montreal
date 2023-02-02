using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MaMontreal.Data;
using MaMontreal.Models;
using MaMontreal.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MaMontreal.Services
{
    public class UsersService : IUsersRepo
    {
        private readonly MamDbContext _context;

        public UsersService(MamDbContext context)
        {
            _context = context;
        }

        public ApplicationUser GetCurrentUser(ClaimsPrincipal user)
        {
            var Username = user.Identity?.Name;

            if (Username == null) return null;

            ApplicationUser CurUser = _context.Users.Where(u => u.NormalizedUserName == Username.ToUpper()).FirstOrDefault<ApplicationUser>();
            // Console.WriteLine(typeof CurUser);
            return CurUser;
        }
    }
}