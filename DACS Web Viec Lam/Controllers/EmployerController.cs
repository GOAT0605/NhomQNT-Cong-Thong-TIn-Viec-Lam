﻿using DACS_Web_Viec_Lam.Data;
using DACS_Web_Viec_Lam.Interface;
using DACS_Web_Viec_Lam.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var jobs = await _context.Job.Include(j => j.Employer).ToListAsync();

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

            return View(jobViewModels);
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

            if (!String.IsNullOrEmpty(searchString))
            {
                string lowercaseSearchString = searchString.ToLower();
                // Tìm kiếm theo title của job thay vì FullName của user
                allRole = allRole.Where(s => s.Title.ToLower().Contains(lowercaseSearchString));
            }

            return View(await allRole.ToListAsync());
        }

    }
}
