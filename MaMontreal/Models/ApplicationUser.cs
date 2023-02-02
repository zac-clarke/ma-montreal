using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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

        // [NotMapped]
        // public IEnumerable<Meeting> MeetingsLead { get; set; }

        // [NotMapped]
        // public IEnumerable<Meeting> MeetingsUpdated { get; set; }
    }
}