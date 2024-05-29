using DACS_Web_Viec_Lam.Models;

namespace DACS_Web_Viec_Lam.Interface
{
    public interface IJobSeekerRepository
    {
        Task<IEnumerable<ApplicationUser>> GetAllAsync();
        Task<ApplicationUser> GetByIdAsync(string userId);
        Task UpdateAsync(ApplicationUser jobseeker);
        Task DeleteAsync(string userId);
        Task AddAsync(JobSeeker product);

        Task<JobSeeker> GetByIdAsync(int id);
    }
    public interface IJobSeeker1Repository
    {
        Task<IEnumerable<JobSeeker>> GetAllAsync();
        Task<ApplicationUser> GetByIdAsync(string userId);
        Task UpdateAsync(ApplicationUser jobseeker);
        Task DeleteAsync(string userId);
        Task AddAsync(JobSeeker product);
    }
}
