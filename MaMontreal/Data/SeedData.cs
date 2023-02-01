using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MaMontreal.Models;
using Microsoft.AspNetCore.Identity;

namespace MaMontreal.Data
{
    public static class SeedData
    {
        public static void SeedRoles(MamDbContext db, RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "admin", "gsr", "member" };
            foreach (string roleName in roleNames)
            {
                if (!roleManager.RoleExistsAsync(roleName).Result)
                {
                    IdentityRole role = new IdentityRole
                    {
                        Name = roleName,
                        NormalizedName = roleName.ToUpper()
                    };
                    IdentityResult result = roleManager.CreateAsync(role).Result;
                }
            }
        }

        public static void AddRole(string userEmail, string role, UserManager<ApplicationUser> userManager)
        {

            var user = userManager.FindByEmailAsync(userEmail).Result;
            
            if (user != null && !userManager.IsInRoleAsync(user, role).Result)
            {
                IdentityResult result = userManager.AddToRoleAsync(user, role).Result;
                if (result.Succeeded)
                    Console.WriteLine($"Added {role} role to: {user.Email}");
                return;
            }
        }

        public static void RemoveRole(string userEmail, string role, UserManager<ApplicationUser> userManager)
        {

            var user = userManager.FindByEmailAsync(userEmail).Result;
            if (user != null && userManager.IsInRoleAsync(user, role).Result)
            {
                IdentityResult result = userManager.RemoveFromRoleAsync(user, role).Result;
                if (result.Succeeded)
                    Console.WriteLine($"Removed {role} role from: {user.Email}");
                return;
            }
        }
    }
}