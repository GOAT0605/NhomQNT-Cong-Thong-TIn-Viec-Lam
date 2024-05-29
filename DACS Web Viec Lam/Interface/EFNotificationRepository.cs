using DACS_Web_Viec_Lam.Data;
using DACS_Web_Viec_Lam.Models;

namespace DACS_Web_Viec_Lam.Interface
{
    public class EFNotificationRepository:INotificationRepository
    {
        private readonly ApplicationDbContext _context;

        public EFNotificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void AddNotification(int userId, string message)
        {
            var notification = new Notification
            {
                UserId = userId,
                Message = message,
                IsRead = false,
                CreatedDate = DateTime.Now
            };
            _context.notifications.Add(notification);
            _context.SaveChanges();
        }

       
    }
}
