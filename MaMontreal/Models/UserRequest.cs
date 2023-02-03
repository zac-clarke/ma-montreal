using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MaMontreal.Models
{
    public class UserRequest : BaseEntity
    {
        public int Id { get; set; }

        //TODO: Removed Required (interferes with form submission that does not have that filled in)
        public IdentityRole? RoleRequested { get; set; }

        // [Required]
        [ForeignKey("RequesteeId")]
        public ApplicationUser? Requestee { get; set; }

        [ForeignKey("RequestHandlerId")]
        public ApplicationUser? RequestHandler { get; set; }

        public bool IsApproved { get; set; } = false;

        // // Using base entity instead
        // [DataType(DataType.DateTime)]
        // public DateTime? RequestDate { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        public DateTime? ProcessedDate { get; set; }

        public string? Note { get; set; }
    }
}