using System.Security.Claims;
using MaMontreal.Models;

namespace MaMontreal.Repositories
{
    public interface IUsersRepo
    {
        public ApplicationUser GetCurrentUser(ClaimsPrincipal user);
    }
}