using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DACS_Web_Viec_Lam.Models
{
    public class Employer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int EmployerId { get; set; }
        //public Employer()
        //{
        //    EmployerId = GenerateRandomId();
        //}
        //private int GenerateRandomId()
        //{
        //    // Generate a random integer using a random number generator
        //    Random rand = new Random();
        //    return rand.Next(100000, 999999); // Adjust the range as needed
        //}
        [Required, StringLength(130)]
       
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string? CompanyDiscription { get; set; }
        public string contactMail { get; set; }

        public string contactPhone { get; set; }
        public string? userId { get; set; }
    }
}
