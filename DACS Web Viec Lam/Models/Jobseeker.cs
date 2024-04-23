using System.ComponentModel.DataAnnotations;

namespace DACS_Web_Viec_Lam.Models
{
    public class JobSeeker
    {
        public string JobSeekerId { get; set; }

        [Required(ErrorMessage = "Please provide a full name.")]
        [StringLength(130, ErrorMessage = "Name must be less than 130 characters.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Please provide an email address.")]
        [StringLength(100, ErrorMessage = "Email must be less than 100 characters.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [StringLength(11, ErrorMessage = "Phone number must be less than or equal to 11 characters.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string PhoneNumber { get; set; }

        [StringLength(500, ErrorMessage = "Description must be less than 500 characters.")]
        public string Description { get; set; }
        public string Experiences { get; set; }

        
        public string Educations { get; set; }

        public Education Education { get; set; }
     
        // public ICollection<Experience> Experiences { get; set; }
        //public ICollection<Education> Educations { get; set; }


        // Additional properties can be added as needed

        // Navigation properties

        // Add more navigation properties as needed
    }

}
