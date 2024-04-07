using System.ComponentModel.DataAnnotations;

namespace DACS_Web_Viec_Lam.Models
{
    public class Education
    {
        public int EducationId { get; set; }

        [Required(ErrorMessage = "Please provide a degree.")]
        public string Degree { get; set; }

        [Required(ErrorMessage = "Please provide a field of study.")]
        public string FieldOfStudy { get; set; }

        [Required(ErrorMessage = "Please provide a school name.")]
        public string SchoolName { get; set; }

        [Required(ErrorMessage = "Please provide a graduation year.")]
        public int GraduationYear { get; set; }

    }
}