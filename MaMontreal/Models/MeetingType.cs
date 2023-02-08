using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaMontreal.Models
{
    public class MeetingType
    {
        public int Id { get; set; }

        [Required, MaxLength(100, ErrorMessage = "You must choose a title no more than 100 characters long")]
        public string Title { get; set; } = null!;

        [NotMapped]
        public IEnumerable<Meeting> Meetings { get; set; } = new List<Meeting>();
    }
}