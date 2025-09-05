using HammerDrop_Auction_app.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HammerDrop_Auction_app.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            var products = _context.Products
                .Include(p => p.Subcategory).ThenInclude(s => s.Category)
                .Include(p => p.Country)
                .Include(p => p.State)
                .Include(p => p.City)
                .Include(p => p.Images);

            return View(await products.ToListAsync());
        }

        public IActionResult Create(int subcategoryId)
        {
            LoadSubcategoryInfo(subcategoryId);
            ViewBag.UserName = User.Identity?.Name ?? "Anonymous";
            LoadLocationDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, List<IFormFile> uploadedImages)
        {
            if (ModelState.IsValid)
            {
                product.Name = User.Identity?.Name ?? "Anonymous";
                _context.Add(product);
                await _context.SaveChangesAsync();

                await SaveUploadedImages(product, uploadedImages);
                return RedirectToAction(nameof(Index));
            }

            LoadSubcategoryInfo(product.SubcategoryId);
            ViewBag.UserName = User.Identity?.Name ?? "Anonymous";
            LoadLocationDropdowns(product.CountryId, product.StateId);
            return View(product);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            LoadSubcategoryInfo(product.SubcategoryId);
            LoadLocationDropdowns(product.CountryId, product.StateId);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product, List<IFormFile> uploadedImages)
        {
            if (id != product.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    await SaveUploadedImages(product, uploadedImages);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Products.Any(e => e.Id == product.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            LoadSubcategoryInfo(product.SubcategoryId);
            ViewBag.UserName = User.Identity?.Name ?? "Anonymous";
            LoadLocationDropdowns(product.CountryId, product.StateId);
            return View(product);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products
                .Include(p => p.Subcategory)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null) return NotFound();

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.Include(p => p.Images).FirstOrDefaultAsync(p => p.Id == id);

            if (product != null)
            {
                foreach (var img in product.Images)
                {
                    var path = Path.Combine(_env.WebRootPath, "images/product", img.ImageName);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);

                    _context.ProductImages.Remove(img);
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
        private void LoadLocationDropdowns(int? countryId = null, int? stateId = null)
        {
            ViewBag.CountryId = _context.Countries
                .OrderBy(c => c.name)
                .Select(c => new SelectListItem
                {
                    Value = c.id.ToString(),
                    Text = c.name,
                    Selected = (countryId != null && c.id == countryId)
                }).ToList();

            ViewBag.StateId = _context.States
                .Where(s => countryId == null || s.country_id == countryId)
                .OrderBy(s => s.name)
                .Select(s => new SelectListItem
                {
                    Value = s.id.ToString(),
                    Text = s.name,
                    Selected = (stateId != null && s.id == stateId)
                }).ToList();

            ViewBag.CityId = _context.Cities
                .Where(c => stateId == null || c.state_id == stateId)
                .OrderBy(c => c.name)
                .Select(c => new SelectListItem
                {
                    Value = c.id.ToString(),
                    Text = c.name
                }).ToList();
        }

        private void LoadSubcategoryInfo(int subcategoryId)
        {
            var subcategory = _context.Subcategories.FirstOrDefault(s => s.Id == subcategoryId);
            ViewBag.SubcategoryTitle = subcategory?.Title ?? "";
            ViewBag.SubcategoryId = subcategoryId;
        }

        private async Task SaveUploadedImages(Product product, List<IFormFile> uploadedImages)
        {
            if (uploadedImages != null && uploadedImages.Count > 0)
            {
                foreach (var file in uploadedImages)
                {
                    if (file.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        var path = Path.Combine(_env.WebRootPath, "images/product", fileName);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        _context.ProductImages.Add(new ProductImage
                        {
                            ProductId = product.Id,
                            ImageName = fileName
                        });
                    }
                }
                await _context.SaveChangesAsync();
            }
        }

        [HttpGet]
        public JsonResult GetStatesByCountry(int countryId)
        {
            var states = _context.States
                .Where(s => s.country_id == countryId)
                .OrderBy(s => s.name)
                .Select(s => new { s.id, s.name })
                .ToList();
            return Json(states);
        }

        [HttpGet]
        public JsonResult GetCitiesByState(int stateId)
        {
            var cities = _context.Cities
                .Where(c => c.state_id == stateId)
                .OrderBy(c => c.name)
                .Select(c => new { c.id, c.name })
                .ToList();
            return Json(cities);
        }
    }
}



