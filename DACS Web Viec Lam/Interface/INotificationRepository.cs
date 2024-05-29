using DACS_Web_Viec_Lam.Models;

namespace DACS_Web_Viec_Lam.Interface
{
    public interface INotificationRepository
    {
        void AddNotification(int userId, string message);
       
    }
}