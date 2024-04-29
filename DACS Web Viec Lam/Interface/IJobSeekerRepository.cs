using DACS_Web_Viec_Lam.Models;

namespace DACS_Web_Viec_Lam.Interface
{
    public interface IJobSeekerRepository
    {
        Task<IEnumerable<JobSeeker>> GetAllAsync();
        Task<JobSeeker> GetByIdAsync(int id);
        Task AddAsync(JobSeeker product);
        Task UpdateAsync(JobSeeker product);
    }
}
