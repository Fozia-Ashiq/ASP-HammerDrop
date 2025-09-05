using Microsoft.EntityFrameworkCore;
using HammerDrop_Auction_app.Entities;
using HammerDrop_Auction_app.Models;
using HammerDrop_Auction_app.Repositories;
using Microsoft.AspNetCore.Mvc;

public class DashboardController : Controller
{
    private readonly IGenericRepository<Listing> _listingRepository;
    private readonly AppDbContext _context;

    public DashboardController(IGenericRepository<Listing> listingRepository, AppDbContext context)
    {
        _listingRepository = listingRepository;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var listings = await _listingRepository.GetAllAsync();
        var products = await _context.Products
            .Include(p => p.Images)
            .Include(p => p.Country)
            .Include(p => p.State)
            .Include(p => p.City)
            .Include(p => p.Subcategory)
            .ToListAsync();

        var model = new DashboardViewModel
        {
            Listings = listings,
            Products = products
        };

        return View(model);
    }
}
