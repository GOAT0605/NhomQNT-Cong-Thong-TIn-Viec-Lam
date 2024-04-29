using DACS_Web_Viec_Lam.Data;
using DACS_Web_Viec_Lam.Models;
using Microsoft.EntityFrameworkCore;

namespace DACS_Web_Viec_Lam.Interface
{
    public class EFJobSeekerRepository:IJobSeekerRepository
    {
        private readonly ApplicationDbContext _context;

        public EFJobSeekerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<JobSeeker>> GetAllAsync()
        {
            return await _context.JobSeeker.ToListAsync();
        }

        public async Task<JobSeeker> GetByIdAsync(int id)
        {
            return await _context.JobSeeker.FindAsync(id);
        }

        public async Task AddAsync(JobSeeker product)
        {
            _context.JobSeeker.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(JobSeeker product)
        {
            _context.JobSeeker.Update(product);
            await _context.SaveChangesAsync();
        }
    }
}
