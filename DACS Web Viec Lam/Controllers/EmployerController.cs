using DACS_Web_Viec_Lam.Data;
using DACS_Web_Viec_Lam.Interface;
using DACS_Web_Viec_Lam.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DACS_Web_Viec_Lam.Controllers
{
    public class EmployerController : Controller
    {

        private readonly IJobRepository _jobRepository;
        private readonly ApplicationDbContext _context;
        public EmployerController(IJobRepository jobRepository, ApplicationDbContext context)

        {
            _jobRepository = jobRepository;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            // Assuming you have a context or repository to access Jobs and Employers
            var jobtemp = await _context.Job
    .Include(j => j.Employer) // Include the related Employer entity
    .Where(j => j.IsDetactive == false) // Filter out inactive jobs
    .ToListAsync();
            var allJob = _context.Job.AsQueryable();
            foreach (var job in jobtemp)
            {
                if (DateTime.Now > job.ApplicationDeadline)
                {
                    job.IsDetactive = true;
                }
            }
            await _context.SaveChangesAsync();
            var jobs = await _context.Job
    .Include(j => j.Employer) // Include the related Employer entity
    .Where(j => j.IsDetactive == false) // Filter out inactive jobs
    .ToListAsync();

            var jobViewModels = jobs.Select(job => new JobViewModel
            {
                JobId = job.JobId,
                Title = job.Title,
                Description = job.Description,
                Location = job.Location,
                Salary = job.Salary,
                Requirement = job.Requirement,
                ApplicationDeadline = job.ApplicationDeadline,
                EmployerName = job.Employer?.CompanyName,
                ImageUrl = job.Employer?.ImageUrl ?? "default-image-url" // Provide a default image URL if not found

            }).ToList();
            var random = new Random();
            jobViewModels = jobViewModels.OrderBy(x => random.Next()).ToList();

            int numberOfJobsToDisplay = random.Next(8, 13);


            var selectedJobViewModels = jobViewModels.Take(numberOfJobsToDisplay).ToList();

            return View(selectedJobViewModels);

        }
        //[Area("JobSeeker")]
        [Authorize(Roles = SD.Role_JobSeeker)]
        public async Task<IActionResult> Display(int id)
        {
            var job = await _context.Job.Include(j => j.Employer).FirstOrDefaultAsync(j => j.JobId == id);
            if (job == null)
            {
                return NotFound();
            }

            var jobViewModel = new JobViewModel
            {
                JobId = job.JobId,
                Title = job.Title,
                Description = job.Description,
                Location = job.Location,
                Salary = job.Salary,
                Requirement = job.Requirement,
                ApplicationDeadline = job.ApplicationDeadline,
                EmployerName = job.Employer?.CompanyName,
                ImageUrl = job.Employer?.ImageUrl ?? "default-image-url"
            };

            return View(jobViewModel);
        }
        public IActionResult Category()
        {
            return View();
        }
        public IActionResult JobDetail()
        {
            return View();
        }
        public IActionResult JobList()
        {
            return View();
        }
        public IActionResult Testimonial()
        {
            return View();
        }

        public async Task<IActionResult> Search(string searchString)
        {
            // Lấy tất cả các employee từ context
            var allRole = from s in _context.Job
                          select s;
            var jobs = await _context.Job
   .Include(j => j.Employer) // Include the related Employer entity
   .Where(j => j.IsDetactive == false) // Filter out inactive jobs
   .ToListAsync();
            var allJob = _context.Job.AsQueryable();

            var jobViewModels = jobs.Select(job => new JobViewModel
            {
                JobId = job.JobId,
                Title = job.Title,
                Description = job.Description,
                Location = job.Location,
                Salary = job.Salary,
                Requirement = job.Requirement,
                ApplicationDeadline = job.ApplicationDeadline,
                EmployerName = job.Employer?.CompanyName,
                ImageUrl = job.Employer?.ImageUrl ?? "default-image-url" // Provide a default image URL if not found

            }).ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                string lowercaseSearchString = searchString.ToLower();
                // Tìm kiếm theo title của job thay vì FullName của user
                jobViewModels = jobViewModels
            .Where(s => s.Title.ToLower().Contains(lowercaseSearchString))
            .ToList(); // Convert to List to avoid casting issue
            }

            return View(jobViewModels);
        }


        public IActionResult NotificationLIst()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Challenge();
            }

            var jobSeeker = _context.JobSeeker
               .FirstOrDefault(j => j.userId == userId);
            var notifications = _context.notifications
                .Where(n => n.UserId == jobSeeker.JobSeekerId && !n.IsRead)
                .OrderByDescending(n => n.CreatedDate)
                .ToList();

            return View(notifications);

        }
        [HttpPost]
        public async Task<IActionResult> MarkAsRead(int notificationId)
        {
            var notification = _context.notifications.FirstOrDefault(j => j.NotificationId == notificationId);

            if (notification != null)
            {
                notification.IsRead = true;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("NotificationList");
        }
    }
}
