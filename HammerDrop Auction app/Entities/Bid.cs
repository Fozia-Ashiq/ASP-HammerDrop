using System.ComponentModel.DataAnnotations;

namespace HammerDrop_Auction_app.Entities
{
    public class Bid
    {
        public int Id { get; set; }

        public int AdId { get; set; }
        public Ad Ad { get; set; }

        public int UserAccountId { get; set; }  // Foreign key to UserAccount
        public UserAccount UserAccount { get; set; }  // Navigation property

        [Range(0.01, double.MaxValue, ErrorMessage = "Bid amount must be greater than zero")]
        public decimal Amount { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Max auto bid must be greater than zero")]
      //  public decimal? MaxAutoBid { get; set; }
        public DateTime BidTime { get; set; }

        public string Status { get; set; } 

    }
}
