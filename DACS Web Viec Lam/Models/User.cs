using System.ComponentModel.DataAnnotations;

namespace DACS_Web_Viec_Lam.Models
{
    public class User
    {
        public string Id { get; set; }

        [Required, StringLength(130)]
        public string Name { get; set; }

        [Required, StringLength(100)]
        public string Email { get; set; }

        [StringLength(11)]
        public string PhoneNumber { get; set; }

        public enum UserType
        {
            Employer,
            Jobseeker
        }

        public string? EmployerId { get; set; }

        public Employer Employer { get; set; }
        public JobSeeker JobSeeker { get; set; }

        public string? JobSeekerId { get; set; }

           }
}
