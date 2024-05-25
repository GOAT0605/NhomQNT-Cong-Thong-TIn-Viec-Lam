using DACS_Web_Viec_Lam.Data;
using DACS_Web_Viec_Lam.Data.Entities;
using DACS_Web_Viec_Lam.Models;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DACS_Web_Viec_Lam.Interface
{
    public class EFTitleRepository : ITitleRepository
    {
        private readonly ApplicationDbContext _context;
        public EFTitleRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Title>> GetAllAsync()
        {
            return await _context.Titles.ToListAsync();
        }

        public async Task<Title> GetByIdAsync(int id)
        {
            return await _context.Titles.FindAsync(id);
        }

        public async Task AddAsync(Title title)
        {
            _context.Titles.Add(title);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Title title)
        {
            _context.Titles.Update(title);
            await _context.SaveChangesAsync();
        }



       
    }
}
