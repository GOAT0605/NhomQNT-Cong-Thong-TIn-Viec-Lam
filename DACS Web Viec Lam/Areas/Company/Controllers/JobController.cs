using DACS_Web_Viec_Lam.Data;
using DACS_Web_Viec_Lam.Interface;
using DACS_Web_Viec_Lam.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DACS_Web_Viec_Lam.Areas.Company.Controllers
{
    [Area("Company")]
    [Authorize(Roles = SD.Role_Company)]
    public class JobController : Controller
    {
        private readonly IJobRepository _jobRepository;
        private readonly ITitleRepository _titleRepository;
        private readonly ApplicationDbContext _context;
        public JobController(IJobRepository JobRepository, ITitleRepository titleRepository, ApplicationDbContext context)
        {
            _jobRepository = JobRepository;
            _titleRepository = titleRepository;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var allJob = _context.Job.AsQueryable();
            //var Employers = await _jobRepository.GetAllAsync();
            //foreach (var title in Employers)
            //{
            //    if (title.TitleId != null)
            //    {
            //        //title.Title = await _titleRepository.GetByIdAsync(title.TitleId);
            //    }
            //}

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var employer = _context.Employers.FirstOrDefault(j => j.userId == userId);

                if (employer == null)
                {
                    // No employer associated with the user, redirect to the Add action in CompanyController
                    return RedirectToAction("Add", "Company");
                }
            // Lọc ra những sản phẩm không bị vô hiệu hoa
            allJob = allJob.Where(p => !p.IsDetactive);
            // Retrieve job entries that match the employer's ID
            var employers = await _jobRepository.GetByUserIdAsync(employer.EmployerId);
                return View(employers);
            

        }
        public async Task<IActionResult> AddAsync()
        {
            var titles = await _titleRepository.GetAllAsync();
            ViewBag.TitleId = new SelectList(titles, "Id", "Name");

            return View();
        }
        [HttpPost]
      public async Task<IActionResult> Add(Job job)
{
    if (ModelState.IsValid)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var EmployerID = _context.Employers.FirstOrDefault(j => j.userId == userId);

        if (EmployerID == null)
        {
            // No Employer ID found, redirect user to Add view
            ModelState.AddModelError("", "You need to create an employer profile first.");
            var title = await _titleRepository.GetAllAsync();
            ViewBag.TitleId = new SelectList(title, "Id", "Name");
            return View(job);
        }

        // Employer ID found, proceed with adding the job
        job.EmployerId = EmployerID.EmployerId;
        await _jobRepository.AddAsync(job);
        return RedirectToAction(nameof(Index));
    }

    // ModelState is not valid, return the Add view with validation errors
         var titles = await _titleRepository.GetAllAsync();
          ViewBag.TitleId = new SelectList(titles, "Id", "Name");
            return View(job);
    }


        public async Task<IActionResult> Display(int id)
        {
            var Employer = await _jobRepository.GetByIdAsync(id);
            if (Employer == null)
            {
                return NotFound();
            }
            return View(Employer);
        }

        // Hiển thị form cập nhật sản phẩm
        public async Task<IActionResult> Update(int id)
        {
            var Employer = await _jobRepository.GetByIdAsync(id);
            if (Employer == null)
            {
                return NotFound();
            }
            var titles = await _titleRepository.GetAllAsync();
            ViewBag.TitleId = new SelectList(titles, "Id", "Name", Employer.TitleId);
            return View(Employer);
        }

      
        [HttpPost]
        public async Task<IActionResult> Update(int id, Models.Job job)
        {


            if (ModelState.IsValid)
            {
                await _jobRepository.UpdateAsync(job);
                return RedirectToAction(nameof(Index));
            }
            return View(job);
        }
        [HttpPost]
        [Authorize(Roles = "Company")]
        public async Task<IActionResult> DeactivateProduct(int id)
        {
            var product = await _context.Job.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            product.IsDetactive = true;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Roles = "Company")]

        public async Task<IActionResult> ReactivateProduct(int id)
        {
            var product = await _context.Job.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            product.IsDetactive = false;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
