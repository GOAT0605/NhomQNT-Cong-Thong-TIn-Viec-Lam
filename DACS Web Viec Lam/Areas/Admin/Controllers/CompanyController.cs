using DACS_Web_Viec_Lam.Data;
using DACS_Web_Viec_Lam.Interface;
using DACS_Web_Viec_Lam.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DACS_Web_Viec_Lam.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IJobSeekerRepository _companyRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        public CompanyController(IJobSeekerRepository jobseekerRepository, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _companyRepository = jobseekerRepository;
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(string searchString)
        {

            var employee = await _userManager.GetUsersInRoleAsync("Company");
            var employeeIds = employee.Select(u => u.Id);
            var allEmployee = from s in _context.users
                              where employeeIds.Contains(s.Id)
                              select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                string lowercaseSearchString = searchString.ToLower();
                allEmployee = allEmployee.Where(s => s.FullName.ToLower().Contains(lowercaseSearchString));
            }

            return View(await allEmployee.ToListAsync());
        }
        public async Task<IActionResult> Edit(string id)
        {
            var jobseekers = await _companyRepository.GetByIdAsync(id);
            if (jobseekers == null)
            {
                return NotFound();
            }
            return View(jobseekers);
        }

        // POST: Updates 
        [HttpPost]
        public async Task<IActionResult> Edit(string id, ApplicationUser jobseekers)
        {
            if (id != jobseekers.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var existingJobseeker = await _companyRepository.GetByIdAsync(id);// Giả định có phương thức GetByIdAsync
                existingJobseeker.FullName = jobseekers.FullName;
                existingJobseeker.UserName = jobseekers.UserName;
                // existingJobseeker.PasswordHash = jobseekers.PasswordHash;
                existingJobseeker.Email = jobseekers.Email;
                await _companyRepository.UpdateAsync(existingJobseeker);
                return RedirectToAction(nameof(Index));

            }
            return View(jobseekers);
        }
        public async Task<IActionResult> LockAccount(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Khóa tài khoản
            var result = await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
            if (result.Succeeded)
            {
                TempData["Message"] = "Khoá Tài Khoản Thành Công";
                return RedirectToAction(nameof(Index));
            }

            return View("Error"); // Hoặc xử lý lỗi phù hợp
        }

        [HttpPost]
        public async Task<IActionResult> UnlockAccount(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            // Mở khóa tài khoản
            var result = await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);
            if (result.Succeeded)
            {
                TempData["Message"] = "Mở Khoá Tài Khoản Thành Công";
                return RedirectToAction(nameof(Index));
            }
            return View("Error"); // Hoặc xử lý lỗi phù hợp
        }
    }
}
