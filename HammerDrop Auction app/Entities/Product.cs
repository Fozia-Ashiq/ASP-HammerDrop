using HammerDrop_Auction_app.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HammerDrop_Auction_app.Entities
{
    public class Product
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

        // New fields :

        [Required(ErrorMessage = "Price is required")]
        public decimal Price { get; set; }

  //      [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Mobile Phone Number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string MobilePhoneNumber { get; set; }

        // Show phone number in ads as image or text
        public bool ShowPhoneNumberInAds { get; set; }

        public ICollection<ProductImage> Images { get; set; }
    }
}
