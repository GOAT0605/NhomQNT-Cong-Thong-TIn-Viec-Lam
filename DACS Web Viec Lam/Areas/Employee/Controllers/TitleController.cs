using DACS_Web_Viec_Lam.Data.Entities;
using DACS_Web_Viec_Lam.Interface;
using DACS_Web_Viec_Lam.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DACS_Web_Viec_Lam.Areas.Employee.Controllers
{
    [Area("Employee")]
    [Authorize(Roles = SD.Role_Employee)]
    public class TitleController : Controller
    {
        private readonly ITitleRepository _titleRepository;
        public TitleController(ITitleRepository titleRepository)
        {
            _titleRepository = titleRepository;
        }

        public async Task<IActionResult> Index()
        {
            var title = await _titleRepository.GetAllAsync();

            return View(title);
        }
        public IActionResult Add()
        {

            return View();
        }

        // Xử lý thêm sản phẩm mới
        [HttpPost]
        public async Task<IActionResult> Add(Title title)
        {
            if (ModelState.IsValid)
            {
                
                await _titleRepository.AddAsync(title);
                return RedirectToAction(nameof(Index));
            }
            return View(title);
        }
        public async Task<IActionResult> Display(int id)
        {
            var product = await _titleRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
    }
}
