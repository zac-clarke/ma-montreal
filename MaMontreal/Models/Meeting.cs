using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaMontreal.Models
{
    public class Meeting
    {
        public int Id { get; set; }

        [Range(minimum: 0, maximum: 512, ErrorMessage = "District # must be between 0 and 512")]
        public UInt16? District { get; set; }

        [Required]
        [MaxLength(60, ErrorMessage = "Event Name cannot be longer than 60 characters")]
        [Display(Name = "Event Name")]
        public string EventName { get; set; }

        [Required]
        [MaxLength(3000, ErrorMessage = "Description cannot be longer than 3000 characters")]
        public string Description { get; set; }

        [Display(Name = "Image Url")]
        public string? ImageUrl { get; set; }

        [Required]
        [MaxLength(255, ErrorMessage = "Address cannot be longer than 255 characters")]
        public string Address { get; set; }

        [Required]
        [MaxLength(64, ErrorMessage = "Citycannot be longer than 64 characters")]
        public string City { get; set; }

        [Required]
        [MaxLength(2, ErrorMessage = "Province cannot be longer than 2 characters")]
        public string Province { get; set; }

        [Required]
        [MaxLength(255, ErrorMessage = "Postal Code cannot be longer than 255 characters")]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [Range(minimum: 0, maximum: 6, ErrorMessage = "Day must be between 0 and 6")]
        public UInt16? Day { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }

        [Required]
        [Display(Name = "End Time")]
        [DataType(DataType.Time)]
        public DateTime EndTime { get; set; }

        [Required]
        [Display(Name = "Meeting Type")]
        public MeetingType MeetingType { get; set; }

        [Required]
        public Language Language { get; set; }

        [Required]
        [ForeignKey("GsrId")]
        public ApplicationUser Gsr { get; set; }

        [Required]
        [Display(Name = "Updated By")]
        [ForeignKey("UpdatedById")]
        public ApplicationUser UpdatedBy { get; set; }

        [Required]
        public Status Status { get; set; }

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Updated At")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Deleted At")]
        public DateTime DeletedAt { get; set; }
    }
}