
using MaMontreal.Data;
using MaMontreal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MaMontreal.Services
{
    public class AdminDashService
    {
        private readonly MamDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminDashService(MamDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (context == null)
                throw new NullReferenceException("Database context is null!");
            if (context.Tags == null)
                throw new NullReferenceException("No Tags table in database!");
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<int> GetTotalUsersAsync()
        {
            return await _context.Users.CountAsync();
        }

        public async Task<AdminDashData> GetAdminDashDataAsync()
        {
            AdminDashData data = new AdminDashData();
            data.roleCountList = await GetListRoleCountAsync();
            data.requestCountList = await GetListRequestCountAsync();
            return data;
        }

        public async Task<List<RoleCount>?> GetListRoleCountAsync()
        {
            List<RoleCount>? roleCountList = new List<RoleCount>();
            var RolesList = _roleManager.Roles.OrderBy(x => x.Name).ToList();

            foreach (var role in RolesList)
            {
                var RolesUserlist = await _userManager.GetUsersInRoleAsync(role.Name?.ToString() ?? "NoRole");
                RoleCount roleCount = new RoleCount();
                roleCount.role = role.Name;
                roleCount.count = RolesUserlist.Count();

                roleCountList.Add(roleCount);
            }
            return roleCountList;
        }

        public async Task<List<RequestCount>?> GetListRequestCountAsync()
        {
            List<RequestCount>? requestCountList = new List<RequestCount>();
            var requesStatustList = new List<bool?> { true, false, null };

            foreach (var status in requesStatustList)
            {
                var requestList = await _context.UserRequests.Where(x => x.IsApproved == status).ToListAsync();
                RequestCount requestCount = new RequestCount();
                requestCount.status = status;
                requestCount.count = requestList.Count();

                requestCountList.Add(requestCount);
            }
            return requestCountList;
        }

        public async Task<bool> GetAnyPendingAsync()
        {
            var requestList = await _context.UserRequests.Where(x => x.IsApproved == null).ToListAsync();
            if (requestList.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        public class AdminDashData
        {
            public List<RoleCount>? roleCountList { get; set; } = new List<RoleCount>();
            public List<RequestCount>? requestCountList { get; set; } = new List<RequestCount>();
        }
        public class RoleCount
        {
            public string? role { get; set; } = "";
            public int? count { get; set; } = 0;
        }
        public class RequestCount
        {
            public bool? status { get; set; } = null;
            public int? count { get; set; } = 0;
        }
    }
}