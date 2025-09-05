using HammerDrop_Auction_app.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HammerDrop_Auction_app.Controllers
{
    [HasPermission(Permission.User)]
    public class BidsController : Controller
    {
        private readonly AppDbContext _context;

        public BidsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> PlaceBid(int AdId, int Amount)
        {
            var ad = await _context.Ads
                .Include(a => a.Bids)
                .FirstOrDefaultAsync(a => a.Id == AdId);

            if (ad == null)
            {
                return NotFound();
            }

            var highestBid = ad.Bids?.OrderByDescending(b => b.Amount).FirstOrDefault()?.Amount
                             ?? ad.BasePrice ?? 0;

            if (Amount <= highestBid)
            {
                TempData["Error"] = "Your bid must be higher than the current bid.";
                return RedirectToAction("Details", "Ads", new { id = AdId });   
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var bid = new Bid
            {
                AdId = AdId,
                UserAccountId = userId,
                Amount = Amount,
                BidTime = DateTime.Now,
                Status = "Active"
            };

            _context.Bids.Add(bid);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Ads", new { id = AdId });
        }
    }
}
