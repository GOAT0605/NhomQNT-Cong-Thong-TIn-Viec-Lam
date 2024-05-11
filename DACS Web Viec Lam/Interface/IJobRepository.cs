using DACS_Web_Viec_Lam.Models;

namespace DACS_Web_Viec_Lam.Interface
{
    public interface IJobRepository
    {
        Task<IEnumerable<Job>> GetAllAsync();
        Task<Job> GetByIdAsync(int id);
        Task AddAsync(Job job);
        Task UpdateAsync(Job job);
        Task<IEnumerable<object>> GetByUserIdAsync(int employerId);
    }
}
