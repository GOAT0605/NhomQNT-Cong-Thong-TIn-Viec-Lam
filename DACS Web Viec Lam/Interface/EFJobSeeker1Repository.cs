using DACS_Web_Viec_Lam.Data;
using DACS_Web_Viec_Lam.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DACS_Web_Viec_Lam.Interface
{
    public class EFJobSeeker1Repository: IJobSeeker1Repository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        public EFJobSeeker1Repository(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IEnumerable<JobSeeker>> GetAllAsync()
        {
            return await _context.JobSeeker.ToListAsync();
        }
        public async Task<ApplicationUser> GetByIdAsync(string userId)
        {
            // Find a user by their ID
            var user = await _userManager.FindByIdAsync(userId);
            return user;
        }

        public async Task UpdateAsync(ApplicationUser jobseeker)
        {
            // Update a user
            await _userManager.UpdateAsync(jobseeker);
            await _context.SaveChangesAsync();
        }
        public async Task AddAsync(JobSeeker jobSeeker)
        {
            _context.JobSeeker.Add(jobSeeker);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string userId)
        {
            // Delete a user by their ID
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
        }
    }
}
