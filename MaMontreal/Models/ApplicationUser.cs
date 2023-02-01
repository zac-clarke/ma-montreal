using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MaMontreal.Models
{
    //It's customary to name this type ApplicationUser
    public class ApplicationUser : IdentityUser // need ASP.NET Core Identity first
    {
        //Data Annotations missing to optimize the database
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? SobrietyDate { get; set; }
    }
}