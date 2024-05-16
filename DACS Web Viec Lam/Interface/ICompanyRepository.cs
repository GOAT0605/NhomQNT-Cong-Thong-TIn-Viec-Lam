using DACS_Web_Viec_Lam.Models;

namespace DACS_Web_Viec_Lam.Interface
{
    public interface ICompanyRepository
    {
        Task<IEnumerable<Employer>> GetAllAsync();
        Task<Employer> GetByIdAsync(int id);
        Task AddAsync(Employer product);
        Task UpdateAsync(Employer product);

        
    }
}
