using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MaMontreal.Models
{
    public class Language
    {
        public int Id { get; set; }

        [Required, MaxLength(30, ErrorMessage = "You must enter a Language with less than 30 characters")]
        public string Title { get; set; } = "Default Language Title";

        [NotMapped]
        public IEnumerable<Meeting> Meetings { get; set; } = new List<Meeting>();

    }
}