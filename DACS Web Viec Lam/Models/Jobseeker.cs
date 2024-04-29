
﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DACS_Web_Viec_Lam.Models
{
    public class JobSeeker
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int JobSeekerId { get; set; }
        //public JobFinder()
        //{
        //    JobSeekerId = GenerateRandomId();
        //}

        //private int GenerateRandomId()
        //{
        //    // Generate a random integer using a random number generator
        //    Random rand = new Random();
        //    return rand.Next(100000, 999999); // Adjust the range as needed
        //}


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


        public string? Educations { get; set; }
        //[Required(ErrorMessage = "Please provide a field of study.")]
        //public string FieldOfStudy { get; set; }


        //[Required(ErrorMessage = "Please provide a school name.")]
        //public string? SchoolName { get; set; }

        //[Required(ErrorMessage = "Please provide a graduation year.")]
        //public int? GraduationYear { get; set; }
      //  public Education Education { get; set; }
        public string? userId { get; set; }
        //public IdentityUser User { get; set; }
    }

}
