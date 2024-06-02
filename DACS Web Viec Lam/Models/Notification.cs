using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DACS_Web_Viec_Lam.Models
{
    public class Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotificationId { get; set; }


        public int? JobseekerId { get; set; }
  
        public int? CompanyId { get; set; }

        public string Message { get; set; }

        public bool IsRead { get; set; } = false;


        public DateTime CreatedDate { get; set; }
    }

}
