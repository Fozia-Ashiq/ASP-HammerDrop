using HammerDrop_Auction_app.Entities;
using HammerDrop_Auction_app.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HammerDrop_Auction_app.Controllers
{
    public class SubCategoryController : Controller
    {
        private readonly IGenericRepository<Subcategory> subcategoryrepository;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly AppDbContext _context;

        public SubCategoryController(IGenericRepository<Subcategory> subcategoryrepository, IWebHostEnvironment webHostEnvironment, AppDbContext context)
        {
            this.subcategoryrepository = subcategoryrepository;
            this.webHostEnvironment = webHostEnvironment;
            _context = context;
        }
        public async Task<IActionResult> Index(int id) 
        {
            var subcategories = await _context.Subcategories
                .Where(s => s.CategoryId == id) 
                .Include(s => s.Category)
                .ToListAsync();

            //Pass category title to view
            ViewBag.CategoryTitle = await _context.Categories
                .Where(c => c.Id == id)
                .Select(c => c.Title)
                .FirstOrDefaultAsync();

            ViewBag.CategoryId = id;
            return View(subcategories);
        }


        public IActionResult Create(int categoryId)
        {
            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Title",categoryId);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Subcategory subcategory)
        {
            if (ModelState.IsValid)
            {
                // Handle image upload
                if (subcategory.Image != null && subcategory.Image.Length > 0)
                {
                    string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images", "subcategory");

                    // Ensure the directory exists
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(subcategory.Image.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await subcategory.Image.CopyToAsync(fileStream);
                    }

                    subcategory.ImageName = uniqueFileName;
                }

                await subcategoryrepository.AddAsync(subcategory);
                return RedirectToAction(nameof(Index));
            }


            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Title", subcategory.CategoryId);

            return View(subcategory);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var subcategory = await subcategoryrepository.GetByIdAsync(id);
            if (subcategory == null)
            {
                return NotFound();
            }
            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Title");
            return View(subcategory);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Subcategory subcategory)
        {
            if (ModelState.IsValid)
            {
                if (subcategory.Image != null && subcategory.Image.Length > 0)
                {
                    string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images", "subcategory");

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }
                    string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(subcategory.Image.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await subcategory.Image.CopyToAsync(fileStream);
                    }
                    subcategory.ImageName = uniqueFileName;
                }
                await subcategoryrepository.UpdateAsync(subcategory);
                return RedirectToAction(nameof(Index));
            }
        ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Title", subcategory.CategoryId);
        return View(subcategory);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var subcategory = await subcategoryrepository.GetByIdAsync(id);
            if (subcategory == null)
            {
                return NotFound();
            }
            return View(subcategory);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await subcategoryrepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        
        public async Task<IActionResult> Details(int id)
        {
            var subcategory = await subcategoryrepository.GetByIdAsync(id);
            if (subcategory == null)
            {
                return NotFound();
            }
            return View(subcategory);
        }
    }
    }
