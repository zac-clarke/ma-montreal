using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MaMontreal.Models.Enums;

namespace MaMontreal.Models
{
    public class Meeting : BaseEntity
    {
        public int Id { get; set; }

        [Range(minimum: 1, maximum: 512, ErrorMessage = "District # must be between 1 and 512")]
        public int District { get; set; }

        [Required]
        [MaxLength(60, ErrorMessage = "Event Name cannot be longer than 60 characters")]
        [Display(Name = "Event Name")]
        public string EventName { get; set; } = null!;

        [Required]
        [MaxLength(3000, ErrorMessage = "Description cannot be longer than 3000 characters")]
        public string Description { get; set; } = null!;

        [Display(Name = "Image Url")]
        public string? ImageUrl { get; set; }

        [Required]
        [MaxLength(255, ErrorMessage = "Address cannot be longer than 255 characters")]
        public string Address { get; set; } = null!;

        [Required]
        [MaxLength(64, ErrorMessage = "City cannot be longer than 64 characters")]
        public string City { get; set; } = null!;

        [Required]
        [Display(Name = "Province Code")]
        public ProvinceCodes ProvinceCode { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Postal Code cannot be shorter than 6 characters")]
        [MaxLength(7, ErrorMessage = "Postal Code cannot be longer than 7 characters")]
        [RegularExpression(@"^([A-Za-z]\d[A-Za-z][ -]?\d[A-Za-z]\d)$", ErrorMessage = "Please enter a valid Canadian Postal Code")]
        [DataType(DataType.PostalCode)]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; } = null!;

        [Display(Name = "Day of Week")]
        [Range(minimum: 0, maximum: 6, ErrorMessage = "Day of Week must be between 0 and 7")]
        public DaysOfWeek? DayOfWeek { get; set; }

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

        // [Required]
        public Statuses? Status { get; set; } = Statuses.Pending;

        [NotMapped]
        [Display(Name = "Language")]
        public int? _LanguageId { get; set; }
        // [Required]
        public Language? Language { get; set; }

        [NotMapped]
        [Display(Name = "Meeting Type")]
        public int? _MeetingTypeId { get; set; }

        // [Required]
        [Display(Name = "Meeting Type")]
        public MeetingType? MeetingType { get; set; }

        // [Required]
        [ForeignKey("GsrId")]
        public ApplicationUser? Gsr { get; set; }

        // [Required]
        [Display(Name = "Updated By")]
        [ForeignKey("UpdatedById")]
        public ApplicationUser? UpdatedBy { get; set; }
    }
}