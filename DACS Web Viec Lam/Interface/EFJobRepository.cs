using DACS_Web_Viec_Lam.Data;
using DACS_Web_Viec_Lam.Models;
using Microsoft.EntityFrameworkCore;

namespace DACS_Web_Viec_Lam.Interface
{
    public class EFJobRepository : IJobRepository
    {
        private readonly ApplicationDbContext _context;

        public EFJobRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Job>> GetAllAsync()
        {
            return await _context.Job.ToListAsync();
        }

        public async Task<Job> GetByIdAsync(int id)
        {
            return await _context.Job.FindAsync(id);
        }

        public async Task AddAsync(Job job)
        {
            _context.Job.Add(job);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Job job)
        {
            _context.Job.Update(job);
            await _context.SaveChangesAsync();
        }

      

        public async Task<IEnumerable<object>> GetByUserIdAsync(int employerId)
        {
            return await _context.Job.Where(j => j.EmployerId == employerId).ToListAsync();
        }

       
    }
 
}
