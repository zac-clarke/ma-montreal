using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MaMontreal.Models
{
    //It's customary to name this type ApplicationUser
    public class ApplicationUser : IdentityUser // need ASP.NET Core Identity first
    {
        [Display(Name = "First Name")]
        [MaxLength(60, ErrorMessage = "Name cannot be longer than 60 characters")]
        public string? FirstName { get; set; }

        [Display(Name = "Last Name")]
        [MaxLength(60, ErrorMessage = "Name cannot be longer than 60 characters")]
        public string? LastName { get; set; }
        public DateTime? SobrietyDate { get; set; }

        public IEnumerable<Meeting> MeetingsLead { get; set; }

        public IEnumerable<Meeting> MeetingsUpdated { get; set; }

        public IEnumerable<UserRequest> UserRequestsSubmitted { get; set; }
        public IEnumerable<UserRequest> UserRequestsHandled { get; set; }
    }
}