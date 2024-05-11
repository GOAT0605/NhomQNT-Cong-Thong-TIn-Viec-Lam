using DACS_Web_Viec_Lam.Data;
using DACS_Web_Viec_Lam.Interface;
using DACS_Web_Viec_Lam.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DACS_Web_Viec_Lam.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class JobSeekerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IJobSeekerRepository _jobseekerRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        public JobSeekerController(IJobSeekerRepository jobseekerRepository, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _jobseekerRepository = jobseekerRepository;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var jobseeker = await _userManager.GetUsersInRoleAsync("JobSeeker");
            var jobseekerIds = jobseeker.Select(x => x.Id).ToList();
            var alljobseeker = from s in _context.JobSeeker where jobseekerIds.Contains(s.userId) select s;
            if (!string.IsNullOrEmpty(searchString))
            {
                string lowercaseSearchString = searchString.ToLower();
                alljobseeker = alljobseeker.Where(s => s.FullName.ToLower().Contains(lowercaseSearchString));
            }
            return View();
        }
        public async Task<IActionResult> Edit(string id)
        {
            var jobseeker = await _jobseekerRepository.GetByIdAsync(id);
            if (jobseeker == null)
            {
                return NotFound();
            }
            return View(jobseeker);
        }

        // POST: Updates 
        [HttpPost]
        public async Task<IActionResult> Edit(string id, ApplicationUser jobseeker)
        {
            if (id != jobseeker.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingjobseeker = await _jobseekerRepository.GetByIdAsync(id); // Giả định có phương thức GetByIdAsync

                existingjobseeker.FullName = jobseeker.FullName;

                existingjobseeker.Email = jobseeker.Email;

                await _jobseekerRepository.UpdateAsync(existingjobseeker);

                return RedirectToAction(nameof(Index));
            }
            return View(jobseeker);
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
