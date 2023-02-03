using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MaMontreal.Models
{
    public class Tag
    {
        public int Id { get; set; }

        [Required, MaxLength(30, ErrorMessage = "Must enter a Tag with no more than 30 characters")]
        public string Title { get; set; }
    }
}