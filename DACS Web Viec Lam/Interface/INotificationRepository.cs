using DACS_Web_Viec_Lam.Models;

namespace DACS_Web_Viec_Lam.Interface
{
    public interface INotificationRepository
    {
        void AddJobSeekerNotification(int userId, string message);
        void AddCompanyNotification(int userId, string message);
    }
}