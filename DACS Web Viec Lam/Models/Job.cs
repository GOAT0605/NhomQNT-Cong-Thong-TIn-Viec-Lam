using System.ComponentModel.DataAnnotations;

namespace DACS_Web_Viec_Lam.Models
{
        public class Job
        {
            public int JobId { get; set; }

            [Display(Name = "Name")]
            [Required(ErrorMessage = "Please enter product name.")]
            [StringLength(100, ErrorMessage = "Job name cannot be more than 100 characters.")]
            public string Title { get; set; }

            [Required(ErrorMessage = "Please provide a job description.")]
            public int TitleId { get; set; }
            [Display(Name = "Description")]
            public string Description { get; set; }

            [Required(ErrorMessage = "Please provide the job location.")]
            public string Location { get; set; }

            [Required(ErrorMessage = "Please provide the job Salary.")]
            [Range(0, double.MaxValue, ErrorMessage = "Salary must be a non-negative value.")]
            public decimal Salary { get; set; }

            [Required(ErrorMessage = "Please provide the job Requirement.")]
            public string Requirement { get; set; }

            [Required(ErrorMessage = "Please specify the application deadline.")]
        
            public DateTime ApplicationDeadline { get; set; }

            public int? EmployerId { get; set; }


            public Employer? Employer { get; set; }
           
        }


}
