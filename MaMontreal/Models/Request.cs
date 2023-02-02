using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MaMontreal.Models
{
    public class Request
    {
        public int Id { get; set; }

        [Required]
        public IdentityRole Role { get; set; }

        [Required]
        public IdentityUser Requestee { get; set; }

        public IdentityUser? RequestHandler { get; set; }

        public bool? IsApproved { get; set; }

        [Required, DataType(DataType.DateTime)]
        public DateTime RequestDate { get; set; } = DateTime.Now;

        public DateTime? ProcessedDate { get; set; }

    }
}