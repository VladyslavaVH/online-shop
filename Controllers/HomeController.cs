using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Models;
using System.Diagnostics;

namespace OnlineShop.Controllers
{
    public class HomeController : Controller
    {
        private ShopDbContext _context;
        private readonly ILogger<HomeController> _logger;
        public string SearchText { get; set; }

        public HomeController(ShopDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Products
                              .Include(p => p.Manufacturer)
                              .Include(p => p.Category)
                              .Include(p => p.Images)
                              .ToListAsync());
        }

        public async Task<IActionResult> Category(string category)
        {
            return View(await _context.Products
                              .Include(p => p.Category)
                              .Include(p => p.Images)
                              .Include(p => p.Manufacturer)
                              .Where(p => p.Category.Name == category)
                              .ToListAsync());
        }

        public async Task<IActionResult> Search(string text)
        {
            text = text.ToLower();

            return View(await _context.Products
                              .Include(p => p.Category)
                              .Include(p => p.Images)
                              .Include(p => p.Manufacturer)
                              .Where(p => p.Name.ToLower().Contains(text))
                              .ToListAsync());
        }

        public IActionResult Product(int id)
        {
            return View( _context.Products
                                  .Where(p => p.Id == id)
                                  .Include(p => p.Category)
                                  .Include(p => p.Images)
                                  .Include(p => p.Manufacturer)
                                  .Single()
                              );
        }

        public IActionResult Contacts()
        {
            return View();
        }
        public IActionResult Delivery()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
