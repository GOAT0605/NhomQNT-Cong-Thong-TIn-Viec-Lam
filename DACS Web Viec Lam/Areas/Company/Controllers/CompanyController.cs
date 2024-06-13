using DACS_Web_Viec_Lam.Data;
using DACS_Web_Viec_Lam.Interface;
using DACS_Web_Viec_Lam.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DACS_Web_Viec_Lam.Areas.Company.Controllers
{
    [Area("Company")]
    [Authorize(Roles = SD.Role_Company)]
    public class CompanyController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICompanyRepository _EmployerRepository;

        public CompanyController(ICompanyRepository EmployerRepository, ApplicationDbContext context)
        {
            _EmployerRepository = EmployerRepository;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var Employers = await _EmployerRepository.GetAllAsync();

            return View(Employers);
        }
        // Hiển thị form thêm sản phẩm mới
        public IActionResult Add()
        {

            return View();
        }

        // Xử lý thêm sản phẩm mới
        [HttpPost]
        public async Task<IActionResult> Add(Models.Employer Employer, IFormFile imageUrl,
List<IFormFile> imageUrls)
        {
            if (ModelState.IsValid)
            {
                if (imageUrl != null)
                {
                    // Lưu hình ảnh đại diện
                    Employer.ImageUrl = await SaveImage(imageUrl);
                }
                if (imageUrls != null)
                {
                    Employer.ImageUrls = new List<EmployerImage>();
                    foreach (var file in imageUrls)
                    {
                        // Lưu các hình ảnh khác
                        Employer.ImageUrl = await SaveImage(imageUrl);
                    }
                }
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                Employer.userId = userId; // Set userId before adding to the repository
                await _EmployerRepository.AddAsync(Employer);
                return RedirectToAction(nameof(Display));
            }
            return View(Employer);
        }
        private async Task<string> SaveImage(IFormFile image)
        {
            var savePath = Path.Combine("wwwroot/images", image.FileName); // Thay đổi đường dẫn theo cấu hình của bạn
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return "/images/" + image.FileName; // Trả về đường dẫn tương đối
        }

        // Hiển thị thông tin chi tiết sản phẩm
        public async Task<IActionResult> Display(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Retrieve the Employer based on the user's ID
            var Employer = _context.Employers
                .FirstOrDefault(j => j.userId == userId);

            if (Employer == null)
            {
                // If no matching Employer found, redirect to the "Add" action
                return RedirectToAction("Add");
            }
            return View(Employer);
        }

        // Hiển thị form cập nhật sản phẩm
        public async Task<IActionResult> Update(int id)
        {
            var Employer = await _EmployerRepository.GetByIdAsync(id);
            if (Employer == null)
            {
                return NotFound();
            }
            return View(Employer);
        }

        // Xử lý cập nhật sản phẩm
        [HttpPost]
        public async Task<IActionResult> Update(int id, Models.Employer Employer)
        {


            if (ModelState.IsValid)
            {
                await _EmployerRepository.UpdateAsync(Employer);
                return RedirectToAction(nameof(Index));
            }
            return View(Employer);
        }
    }
}
