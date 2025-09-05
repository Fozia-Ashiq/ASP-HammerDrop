using HammerDrop_Auction_app.Entities;
using HammerDrop_Auction_app.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HammerDrop_Auction_app.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IGenericRepository<Category> categoryrepository;
        private readonly IWebHostEnvironment webHostEnvironment;

        public CategoryController(IGenericRepository<Category> categoryrepository, IWebHostEnvironment webHostEnvironment)
        {
            this.categoryrepository = categoryrepository;
            this.webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await categoryrepository.GetAllAsync();
            return View(categories);
        }
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.Image != null && category.Image.Length > 0)
                {
                    string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images", "category");

                    // Ensure the directory exists
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(category.Image.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await category.Image.CopyToAsync(fileStream);
                    }

                    category.ImageName = uniqueFileName;
                }

                await categoryrepository.AddAsync(category);  // Don't forget to await this
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await categoryrepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                if (category.Image != null)
                {
                    string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath,"images", "category");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + category.Image.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        category.Image.CopyTo(fileStream);
                    }
                    category.ImageName = uniqueFileName;
                }
                await categoryrepository.UpdateAsync(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var category = await categoryrepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost, ActionName("Delete")]   //treat it like a dELETE when routing
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await categoryrepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
