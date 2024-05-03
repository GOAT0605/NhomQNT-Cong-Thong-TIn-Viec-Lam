using DACS_Web_Viec_Lam.Interface;
using DACS_Web_Viec_Lam.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DACS_Web_Viec_Lam.Areas.Company.Controllers
{
    [Area("Company")]
    [Authorize(Roles = SD.Role_Company)]
    public class JobController : Controller
    {
        private readonly IJobRepository _jobRepository;

        public JobController(IJobRepository JobRepository)
        {
            _jobRepository = JobRepository;
        }
        public async Task<IActionResult> Index()
        {
            var Employers = await _jobRepository.GetAllAsync();

            return View(Employers);
        }
        public IActionResult Add()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(Models.Job job)
        {
            if (ModelState.IsValid)
            {
                await _jobRepository.AddAsync(job);
                return RedirectToAction(nameof(Index));
            }
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
    }
}
