using DACS_Web_Viec_Lam.Models;

namespace DACS_Web_Viec_Lam.Repositories
{
    public interface IJobSeekerRepository
    {
        Task AddAsync(JobSeeker jobSeeker);
        bool Exists(string userId);
    }
}
