using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MaMontreal.Models
{
    public class MeetingType
    {
        public int Id { get; set; }
        [Required, MaxLength(100,
        ErrorMessage = "You must choose a title no more than 100 characters long")]
        public string Title { get; set; }

    }
}