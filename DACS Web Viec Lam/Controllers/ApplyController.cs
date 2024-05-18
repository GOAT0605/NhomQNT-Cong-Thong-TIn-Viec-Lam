using DACS_Web_Viec_Lam.Data;
using DACS_Web_Viec_Lam.Data.Entities;
using DACS_Web_Viec_Lam.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;

namespace DACS_Web_Viec_Lam.Controllers
{
    public class ApplyController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ApplyController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = SD.Role_JobSeeker)]
        [HttpGet]
        public IActionResult Add(int id)
        {
            var job = _context.Job.FirstOrDefault(j => j.JobId == id);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var jobSeeker = _context.JobSeeker.FirstOrDefault(j => j.userId == userId);
            return View(jobSeeker);
        }

        [Authorize(Roles = SD.Role_JobSeeker)]
        [HttpPost]
        public async Task<IActionResult> Add(ApplicationList applicationList, int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var jobSeeker = _context.JobSeeker.FirstOrDefault(j => j.userId == userId);

            if (jobSeeker != null)
            {
                applicationList = new ApplicationList
                {
                    ApplicationDate = DateTime.Now,
                    JobSeekerId = jobSeeker.JobSeekerId,
                    JobId = id,
                };

                _context.applicationLists.Add(applicationList);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(ApplyQuene));
            }
            else
            {
                return RedirectToAction("Add", "JobSeeker");
            }
        }

        public async Task<IActionResult> ApplyQuene()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var jobSeeker = await _context.JobSeeker.FirstOrDefaultAsync(j => j.userId == userId);

            if (jobSeeker == null)
            {
                return NotFound("Job seeker not found");
            }

            var cvs = await _context.applicationLists.Where(j => j.JobSeekerId == jobSeeker.JobSeekerId).ToListAsync();

            if (cvs == null || !cvs.Any())
            {
                return NotFound("No applications found");
            }

            var jobs = await _context.Job.Where(j => cvs.Select(cv => cv.JobId).Contains(j.JobId)).ToListAsync();

            var cvViewModels = cvs.Select(cv =>
            {
                var job = jobs.FirstOrDefault(j => j.JobId == cv.JobId);
                return new CVViewModel
                {
                    JobId = (int)cv.JobId,
                    Title = job?.Title,
                    JobDescription = job?.Description,
                    Requirement = job?.Requirement,
                    ApplicationDeadline = (DateTime)(job?.ApplicationDeadline),
                    ApplicationDate = cv.ApplicationDate,
                    ApplicationId = cv.ApplicationId,
                    Status = cv.Status,
                    ImageUrl = jobSeeker.ImageUrl,
                    JobSeekerId = jobSeeker.JobSeekerId,
                    FullName = jobSeeker.FullName,
                    PhoneNumber = jobSeeker.PhoneNumber,
                    Email = jobSeeker.Email,
                    Educations = jobSeeker.Educations,
                    Description = jobSeeker.Description,
                    Experiences = jobSeeker.Experiences,
                };
            }).ToList();

            return View(cvViewModels);
        }


        [Authorize(Roles = SD.Role_Company)]
        public async Task<IActionResult> Apply()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var company = await _context.Employers.FirstOrDefaultAsync(j => j.userId == userId);

            if (company == null)
            {
                return NotFound("Company not found.");
            }

            var jobs = await _context.Job.Where(j => j.EmployerId == company.EmployerId).ToListAsync();

            if (!jobs.Any())
            {
                return NotFound("No jobs found for this company.");
            }

            var jobIds = jobs.Select(j => j.JobId).ToList();
            var cvs = await _context.applicationLists.Where(a => jobIds.Contains((int)a.JobId)).ToListAsync();

            if (!cvs.Any())
            {
                return NotFound("No applications found.");
            }

            var jobSeekerIds = cvs.Select(a => a.JobSeekerId).Distinct().ToList();
            var jobSeekers = await _context.JobSeeker.Where(js => jobSeekerIds.Contains(js.JobSeekerId)).ToListAsync();

            var cvViewModels = cvs.Select(cv =>
            {
                var job = jobs.FirstOrDefault(j => j.JobId == cv.JobId);
                var jobSeeker = jobSeekers.FirstOrDefault(js => js.JobSeekerId == cv.JobSeekerId);

                return new CVViewModel
                {
                    JobId = (int)cv.JobId,
                    Title = job?.Title,
                    JobDescription = job?.Description,
                    Requirement = job?.Requirement,
                    ApplicationDeadline = job?.ApplicationDeadline ?? DateTime.MinValue,
                    ApplicationDate = cv.ApplicationDate,
                    ApplicationId = cv.ApplicationId,
                    Status = cv.Status,
                    ImageUrl = jobSeeker?.ImageUrl,
                    JobSeekerId = jobSeeker?.JobSeekerId ?? 0,
                    FullName = jobSeeker?.FullName,
                    PhoneNumber = jobSeeker?.PhoneNumber,
                    Email = jobSeeker?.Email,
                    Educations = jobSeeker?.Educations,
                    Description = jobSeeker?.Description,
                    Experiences = jobSeeker?.Experiences,
                };
            }).ToList();

            return View(cvViewModels);
        }

    }
}
