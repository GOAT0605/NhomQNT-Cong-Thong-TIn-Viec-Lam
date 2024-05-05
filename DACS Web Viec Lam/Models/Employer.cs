using System.ComponentModel.DataAnnotations;

namespace DACS_Web_Viec_Lam.Models
{
    public class Employer
    {
        public string EmployerId { get; set; }
        [Required, StringLength(130)]
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string? CompanyDiscription { get; set; }
        public string contactMail { get; set; }

        public string contactPhone { get; set; }
        public string? userId { get; set; }
        public string? ImageUrl { get; set; } // Đường dẫn đến hình ảnh đại diện
        public List<EmployerImage>? ImageUrls { get; set; }
    }
}
