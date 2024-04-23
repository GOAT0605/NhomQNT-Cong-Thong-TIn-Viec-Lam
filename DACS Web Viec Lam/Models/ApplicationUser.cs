using Microsoft.AspNetCore.Identity;

namespace DACS_Web_Viec_Lam.Models
{
    public class ApplicationUser
    {
        public string FullName { get; set; }
        public string? Address { get; set; }
        public string? Age { get; set; }

    }
}
