using System.ComponentModel.DataAnnotations;

namespace DACS_Web_Viec_Lam.Models
{
    public class Education
    {
        public int EducationId { get; set; }

        [Required(ErrorMessage = "Please provide a degree.")]
        public string Degree { get; set; }

       

    }
}