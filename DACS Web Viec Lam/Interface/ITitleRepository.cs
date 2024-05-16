using DACS_Web_Viec_Lam.Data.Entities;
using DACS_Web_Viec_Lam.Models;

namespace DACS_Web_Viec_Lam.Interface
{
    public interface ITitleRepository
    {
        Task<IEnumerable<Title>> GetAllAsync();
        Task<Title> GetByIdAsync(int id);
        Task AddAsync(Title title);
        Task UpdateAsync(Title title);
    }
}
