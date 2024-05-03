using DACS_Web_Viec_Lam.Data;
using DACS_Web_Viec_Lam.Models;
using Microsoft.EntityFrameworkCore;

namespace DACS_Web_Viec_Lam.Interface
{
    public class EFCompanyRepository : ICompanyRepository
    {
        private readonly ApplicationDbContext _context;

        public EFCompanyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employer>> GetAllAsync()
        {
            return await _context.Employers.ToListAsync();
        }

        public async Task<Employer> GetByIdAsync(int id)
        {
            return await _context.Employers.FindAsync(id);
        }

        public async Task AddAsync(Employer product)
        {
            _context.Employers.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Employer product)
        {
            _context.Employers.Update(product);
            await _context.SaveChangesAsync();
        }

        
    }
}
