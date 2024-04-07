using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;


namespace DACS_Web_Viec_Lam.Data.Entities
{
    public class UserRole : IdentityRole<Guid>
    {
        [Display(Name = "Description")]
        [StringLength(256, ErrorMessage = "The description cannot be more than 256 characters.")]
        public string? Description { get; set; }
    }
}