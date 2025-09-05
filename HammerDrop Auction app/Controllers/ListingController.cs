using HammerDrop_Auction_app.Entities;
using HammerDrop_Auction_app.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HammerDrop_Auction_app.Models;

namespace HammerDrop_Auction_app.Controllers
{
    public class ListingController : Controller
    {
        private readonly IGenericRepository<Listing> _listingRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly AppDbContext _context;

        public ListingController(IGenericRepository<Listing> listingRepository, IWebHostEnvironment webHostEnvironment,AppDbContext context)
        {
            _listingRepository = listingRepository;
            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            var listings = await _listingRepository.GetAllAsync();
            ViewBag.Username = User.Identity.Name;
            return View(listings);
        }
    
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Listing listing)
        {
            if (ModelState.IsValid)
            {
                // Handle image upload
                if (listing.ImageFile != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + listing.ImageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await listing.ImageFile.CopyToAsync(fileStream);
                    }

                    listing.ImageName = uniqueFileName;
                }

                await _listingRepository.AddAsync(listing);
                return RedirectToAction(nameof(Index));
            }

            return View(listing);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var listing = await _listingRepository.GetByIdAsync(id);
            if (listing == null)
            {
                return NotFound();
            }

            return View(listing);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Listing listing)
        {
            if (id != listing.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Handle image update if new image uploaded
                if (listing.ImageFile != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + listing.ImageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await listing.ImageFile.CopyToAsync(fileStream);
                    }

                    listing.ImageName = uniqueFileName;
                }

                await _listingRepository.UpdateAsync(listing);
                return RedirectToAction(nameof(Index));
            }

            return View(listing);
        }

        //public async Task<IActionResult> Dashboard()
        //{
        //    var listings = await _listingRepository.GetAllAsync();
        //    var products = await _context.Products
        //        .Include(p => p.Subcategory)
        //        .Include(p => p.Images)
        //        .ToListAsync();

        //    var model = new DashboardViewModel
        //    {
        //        Listings = listings,
        //        Products = products
        //    };

        //    return View(model);
        //}

        //public async Task<IActionResult> Details(int id)
        //{
        //    var listing = await _listingRepository.GetByIdAsync(id);
        //    if (listing == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(listing);
        //}


        public async Task<IActionResult> Delete(int id)
        {
            var listing = await _listingRepository.GetByIdAsync(id);
            if (listing == null)
            {
                return NotFound();
            }

            return View(listing);
        }

        [HttpPost, ActionName("Delete")]   //treat it like a dELETE when routing
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _listingRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
