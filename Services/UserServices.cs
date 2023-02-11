using System.Security.Claims;
using MaMontreal.Data;
using MaMontreal.Models;
using MaMontreal.Models.NotMapped;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MaMontreal.Services
{

    public class UsersService
    {
        private readonly MamDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;




        public UsersService(MamDbContext context,
        UserManager<ApplicationUser> userManager)
        {
            if (context == null)
                throw new NullReferenceException("Database context is null!");
            if (context.Users == null)
                throw new NullReferenceException("No Users table in database!");
            _context = context;
            if (userManager == null)
                throw new NullReferenceException("userManager is null!");
            _userManager = userManager;
        }

        public async Task<ApplicationUser?> GetCurUserAsync(ClaimsPrincipal User)
        {
            return await _userManager.GetUserAsync(User);
        }

        public async Task<List<ApplicationUser>> GetAllAsync()
        {
            List<ApplicationUser> users = await _context.Users.ToListAsync();
            return users ?? new List<ApplicationUser>();
        }


        public async Task<ApplicationUser> GetAsync(string id)
        {
            if (id == null)
            {
                throw new NullReferenceException("Parameter 'id' is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                throw new NullReferenceException("User not found.");
            }
            return user;
        }

        public async Task UpdateRolesForUserAsync(string id, UserWithRoles userWithRoles)
        {
            var user = await this.GetAsync(id);
            if (user.FirstName == null || user.LastName == null || user.PhoneNumber == null || user.SobrietyDate == null)
                throw new NullReferenceException("Not updated: User Profile must be complete with Full Name, Phone Numevr and Sobriety Date before updating role to other than Member.");

            foreach (var role in userWithRoles._selectedRoles)
            {
                if (role._roleSelected)
                {
                    await _userManager.AddToRoleAsync(user, role._roleName);
                }
                else
                    await _userManager.RemoveFromRoleAsync(user, role._roleName);
            }
        }



        public async Task<UserWithRoles> GetUserWithRolesAsync(string id)
        {
            var user = await this.GetAsync(id);
            var userRoles = await _userManager.GetRolesAsync(user);
            var appRoles = await _context.Roles.Select(x => x.Name).ToListAsync();
            List<ManagedRole> currentUserRoles = new List<ManagedRole>();
            foreach (var role in appRoles)
            {
                ManagedRole newRole = new ManagedRole()
                {
                    _roleName = role == null ? "unknown" : role,
                    _roleSelected = false
                };
                if (role != null && userRoles.Contains(role))
                    newRole._roleSelected = true;

                currentUserRoles.Add(newRole);
            }
            UserWithRoles userWithRoles = new UserWithRoles()
            {
                _userId = user.Id,
                _selectedRoles = currentUserRoles
            };

            return userWithRoles;
        }

        public async Task DeleteAsync(string id)
        {
            var user = await this.GetAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }


        public async Task<List<ApplicationUser>?> GetUsersWithRole(string roleName)
        {
            string? gsrRoleId = _context.Roles?
                                        .Where(r => r.Name != null && r.Name.ToLower().Equals(roleName.ToLower()))?
                                        .FirstOrDefaultAsync()?
                                        .Result?
                                        .Id;
            List<string> userIds = await _context.UserRoles?
                                                    .Where(ur => ur.RoleId.Equals(gsrRoleId))
                                                    .Select(ur => ur.UserId)
                                                    .ToListAsync<string>()!;
            List<ApplicationUser> users = new List<ApplicationUser>();
            foreach (var userId in userIds)
            {
                users.Add(await this.GetAsync(userId));
            }
            return users;
        }
    }
}