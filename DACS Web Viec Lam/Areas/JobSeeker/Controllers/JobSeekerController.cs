using DACS_Web_Viec_Lam.Data;
using DACS_Web_Viec_Lam.Models;
using DACS_Web_Viec_Lam.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System.Security.Claims;
using System.Text;

namespace DACS_Web_Viec_Lam.Areas.JobSeeker.Controllers
{
    [Area("JobSeeker")]
    [Authorize(Roles = "JobSeeker")]
    public class JobSeekerController : Controller
    {
        private readonly IJobSeekerRepository _jobSeekerRepository;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly Random _random = new Random();
        private const string CharSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        public JobSeekerController(ApplicationDbContext context, UserManager<IdentityUser> userManager,
            IJobSeekerRepository jobSeekerRepository)
        {
            _context = context;
            _userManager = userManager;
            _jobSeekerRepository = jobSeekerRepository;
        }
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Retrieve the JobSeeker based on the user's ID
            var jobSeeker = _context.JobSeeker
                .Include(j => j.User) // Include if there's a navigation property to the User table
                .FirstOrDefault(j => j.userId == userId);

            if (jobSeeker == null)
            {
                return RedirectToAction("Add");
            }

            return View(jobSeeker); // Pass the JobSeeker entity to the view
           
        }
        public IActionResult AddAsync()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Models.JobSeeker jobSeeker)
        {
            if (ModelState.IsValid)
            {
                var userinfo = await _userManager.GetUserAsync(User);

                if (userinfo != null)
                {
                    // Generate a unique user ID
                    //string userId;
                    //do
                    //{
                    //    userId = GenerateRandomString(20); // Adjust the length as needed
                    //} while (_jobSeekerRepository.Exists(userId) || _context.JobSeeker.Any(j => j.JobSeekerId == userId));
                    //jobSeeker.JobSeekerId = userId;
                    jobSeeker.userId = userinfo.Id;
                
                    await _jobSeekerRepository.AddAsync(jobSeeker);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return RedirectToAction("Error", "Home");
                }
            }

            return View(jobSeeker);

        }

        private string GenerateRandomString(int length)
        {
            StringBuilder sb = new StringBuilder();

            // Generate random characters for the string
            for (int i = 0; i < length; i++)
            {
                int index = _random.Next(CharSet.Length);
                sb.Append(CharSet[index]);
            }

            return sb.ToString();
        }
    }
}
