using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HammerDrop_Auction_app.Attributes;

namespace HammerDrop_Auction_app.Entities
{
    public class Ad
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(1000, ErrorMessage = "Description can't exceed 1000 characters")]
        public string Description { get; set; }

        [Required]
        public int SubcategoryId { get; set; }
        public Subcategory Subcategory { get; set; }

        public string BrandName { get; set; }

        [Required]
        public int CountryId { get; set; }
        public Country Country { get; set; }

        [Required]
        public int StateId { get; set; }
        public State State { get; set; }

        [Required]
        public int CityId { get; set; }
        public City City { get; set; }

        public decimal? Price { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Mobile Phone Number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string MobilePhoneNumber { get; set; }

       // [RegularExpression(@"^\+?[1-9]\d{7,14}$", ErrorMessage = "Enter a valid international phone number")]
        public bool ShowPhoneNumberInAds { get; set; }

        public ICollection<AdImages> Images { get; set; }

        //Auction Feature
        public bool IsAuction { get; set; }

        public ICollection<Bid> Bids { get; set; }


        //if IsAuction == true
        [Range(0, double.MaxValue, ErrorMessage = "Base price must be positive")]

        [RequiredIf(nameof(IsAuction), true, ErrorMessage = "Base price is required for auction ads")]
        public decimal? BasePrice { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Reserved price must be positive")]
        [RequiredIf(nameof(IsAuction), true, ErrorMessage = "Reserved price is required for auction ads")]
        public decimal? ReservedPrice { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Minimum increment must be positive")]

        [RequiredIf(nameof(IsAuction), true, ErrorMessage = "Duration is required for auction ads")]
        public DateTime? AuctionEndTime { get; set; }
        //    public decimal? MinimumBidIncrement { get; set; }

        //   public decimal? CurrentHighestBid { get; set; }

        // Proxy Bidding
        //  public bool EnableProxyBidding { get; set; }

        // Auction Status
        //   public string AuctionStatus { get; set; }

        public int UserAccountId { get; set; }

        public UserAccount UserAccount { get; set; }

    }
}
