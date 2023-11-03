using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Models;

namespace OnlineShop.Controllers
{
    [Authorize(Roles = "admin")]
    public class ShopManagerController : Controller
    {
        private readonly ShopDbContext _context;
        private readonly IWebHostEnvironment _appEnvironment;

        public ShopManagerController(ShopDbContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        async public Task<IActionResult> Index()
        {
            return View(await _context.Products
                              .Include(p => p.Category)
                              .Include(p => p.Manufacturer)
                              .ToListAsync());
        }

        public IActionResult Create()
        {
            ViewBag.CategoryFK = _context.Categories.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList();
            ViewBag.ManufacturerFK = _context.Manufacturers.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create([Bind("Name", "Color", "Count", "Price", "CategoryFK", "ManufacturerFK", "Description")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryFK = _context.Categories.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList();
            ViewBag.ManufacturerFK = _context.Manufacturers.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList();
            return View(product);
        }

        public ActionResult Edit(int id)
        {
            Product product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            var categories = _context.Categories.Select(c => 
                                                            new SelectListItem 
                                                            { 
                                                                Value = c.Id.ToString(), 
                                                                Text = c.Name
                                                            }).ToList();
            foreach (var c in categories)
            {
                c.Selected = int.Parse(c.Value) == product.CategoryFK;
            }

            ViewBag.CategoryFK = categories;
            var manufacturers = _context.Manufacturers.Select(c =>
                                                new SelectListItem
                                                {
                                                    Value = c.Id.ToString(),
                                                    Text = c.Name
                                                }).ToList();

            foreach (var m in manufacturers)
            {
                m.Selected = int.Parse(m.Value) == product.ManufacturerFK;
            }

            ViewBag.ManufacturerFK = manufacturers;
            return View(product);
        }

        [HttpPost]
        public IActionResult Edit([Bind("Id", "Name", "Color", "Count", "Price", "CategoryFK", "ManufacturerFK", "Description")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(product).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryFK = _context.Categories.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList();
            ViewBag.ManufacturerFK = _context.Manufacturers.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList();
            return View(product);
        }

        async public Task<IActionResult> Delete(int id)
        {
            Product product = await FindProduct(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteProduct(int id)
        {
            Product product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        async public Task<IActionResult> Details(int id)
        {
            Product product = await FindProduct(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public ActionResult Images(int id)
        {
            Product product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(_context.Images
                                 .Where(i => i.ProductFK == id)
                                 .ToList());
        }

        [HttpPost]
        public async Task<IActionResult> AddImage(IFormFile uploadedImage, int id)
        {
            if (uploadedImage != null)
            {
                // путь к папке images
                string path = "/images/" + uploadedImage.FileName;
                // сохраняем файл в папку images в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedImage.CopyToAsync(fileStream);
                }

                Image image = new() { Url = path, ProductFK = id };
                _context.Images.Add(image);
                _context.SaveChanges();
            }

            return RedirectToAction("Images", "ShopManager", new { id });
        }

        public IActionResult DeleteImage(int imageId, int productId)
        {
            Image image = _context.Images.Find(imageId);
            string imgPath = _appEnvironment.WebRootPath + image.Url;
            FileInfo fileInfo = new(imgPath);
            if (fileInfo.Exists)
            {
                System.IO.File.Delete(imgPath);
                fileInfo.Delete();
                _context.Images.Remove(image);
                _context.SaveChanges();
            }
            return RedirectToAction("Images", "ShopManager", new { id = productId });
        }

        async private Task<Product> FindProduct(int id)
        {
            Product product = await _context.Products.FindAsync(id);
            _context.Entry(product).Reference(p => p.Category).Load();
            _context.Entry(product).Reference(p => p.Manufacturer).Load();

            return product;
        }
    }
}