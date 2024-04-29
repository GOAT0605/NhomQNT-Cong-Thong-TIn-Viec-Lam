using DACS_Web_Viec_Lam.Data;
using DACS_Web_Viec_Lam.Models;
using Microsoft.EntityFrameworkCore;

namespace DACS_Web_Viec_Lam.Repositories
{
    public class EFJobSeekerRepository : IJobSeekerRepository
    {
        private readonly ApplicationDbContext _context;
        public EFJobSeekerRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(JobSeeker jobSeeker)
        {
           _context.JobSeeker.Add(jobSeeker);
            await _context.SaveChangesAsync();
        }
        public bool Exists(string userId)
        {
            // Check if a JobSeeker with the given userId already exists in the database
            return _context.JobSeeker.Any(j => j.JobSeekerId == userId);
        }
    }
}
