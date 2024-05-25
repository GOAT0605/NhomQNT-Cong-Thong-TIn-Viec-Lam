using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DACS_Web_Viec_Lam.Models
{

    public class ApplicationList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int ApplicationId { get; set; }

        public DateTime? ApplicationDate { get; set; }

        [Required]
        public ApplicationStatus Status { get; set; } = ApplicationStatus.NotChecked;
        public string? comment { get; set; }
        public int? JobSeekerId { get; set; }
        public JobSeeker? JobSeeker { get; set; }

        public int? JobId { get; set; }
        public Job Job { get; set; }
    }

    public enum ApplicationStatus
    {
        NotChecked = 0,
        Applied = 1,
        Declined = 2
    }
}
