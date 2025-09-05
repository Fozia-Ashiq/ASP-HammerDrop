using HammerDrop_Auction_app.Entities;

namespace HammerDrop_Auction_app.Models
{
    public class DashboardViewModel
    {
        public IEnumerable<Listing> Listings { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
