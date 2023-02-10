using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MaMontreal.Data;
using MaMontreal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MaMontreal.Services
{
    public class RequestsService
    {
        private MamDbContext _context;
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<ApplicationUser> _userManager = null!;

        public RequestsService(
            MamDbContext context,
        RoleManager<IdentityRole> roleManager,
        UserManager<ApplicationUser> userManager)
        {
            if (context == null)
                throw new NullReferenceException("Database context is null!");
            if (context.UserRequests == null)
                throw new NullReferenceException("No UserRequests table in database!");
            _context = context;
            if (roleManager == null)
                throw new NullReferenceException("Role Manager is null!");
            _roleManager = roleManager;
            if (userManager == null)
                throw new NullReferenceException("User Manager is null!");
            _userManager = userManager;
        }




        public async Task CreateAsync(ClaimsPrincipal User, UserRequest request, string? role)
        {

            ApplicationUser? currUser = await _userManager.GetUserAsync(User);
            if (currUser == null)
                throw new NullReferenceException("User not found.");

            // Check if currUser has a FullName, PhoneNumber and SobrietyDate and if not throw only one exception for all to be caught by the controller
            //Testing Copilot to write this code. It's not perfect but it's a start. but i still like it. even if I am the only one who will ever see it.
            // Both the comment above and the if sttatement below were written by Copilot. :)))) Daaaaamn Copilot is good.
            if (currUser.FirstName == null || currUser.LastName == null || currUser.PhoneNumber == null || currUser.SobrietyDate == null)
                throw new NullReferenceException("Make sure that your Profile is complete with your Full Name, Phone Numevr and Sobriety Date before requesting this role.");

            //all good
            UserRequest userRequest = new UserRequest(_roleManager, currUser, role, request.Note);
            _context.Add(userRequest);
            await _context.SaveChangesAsync();
        }


        internal async Task HandleAsync(int? id, ClaimsPrincipal User, bool? isApproved)
        {
            if (id == null)
                throw new NullReferenceException("No Request Found");

            if (isApproved == null)
                throw new NullReferenceException("No Meaningful Action taken");

            ApplicationUser? reqHandler = await _userManager.GetUserAsync(User);
            if (reqHandler == null || _userManager.IsInRoleAsync(reqHandler, "admin").Result == false)
                throw new NullReferenceException("You don't have permission to approve this request.");

            UserRequest? request = await _context.UserRequests.Where(r => r.Id == id).Include(r => r.RoleRequested).Include(r => r.Requestee).FirstOrDefaultAsync();

            if (request == null)
                throw new NullReferenceException("No Request Found");

            if (request.Requestee == null)
                throw new NullReferenceException("Requestee User Not Found");

            ApplicationUser? user = await _userManager.FindByIdAsync(request.Requestee.Id);
            if (user == null)
                throw new NullReferenceException("Requestee User Not Found");

            if (request.RoleRequested?.Name == null)
                throw new NullReferenceException("Request Role Not Found");

            if (isApproved == true)
                await _userManager.AddToRoleAsync(user, request.RoleRequested.Name);

            request.IsApproved = isApproved;
            request.RequestHandler = reqHandler;
            request.UpdatedAt = DateTime.Now;
            request.ProcessedDate = DateTime.Now;
            _context.Update(request);
            await _context.SaveChangesAsync();
        }


        public async Task<List<UserRequest>> GetAllAsync()
        {
            return await _context.UserRequests.Include("Requestee").Include("RequestHandler").Include("RoleRequested").OrderByDescending(r => r.CreatedAt).ToListAsync();
        }
    }
}