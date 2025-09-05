using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HammerDrop_Auction_app.Entities
{
    public class Listing
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        [MaxLength(150, ErrorMessage = "Name can't be longer than 150 characters")]
        public string Name { get; set; }

        [Required,MaxLength(1000, ErrorMessage = "Description can't be longer than 1000 characters")]
        public string? Description { get; set; }

        public string ImageName { get; set; }

        // This is for image upload only — not saved in DB
        [NotMapped,Required]
        //  [Required(ErrorMessage = "Please upload a product image")]
        [DataType(DataType.Upload)]
        public IFormFile? ImageFile { get; set; }


        //// Foreign key
        //[ForeignKey("Category")]
        //[Required(ErrorMessage = "Please select a category")]
        //public int CategoryId { get; set; }

        //// Navigation
        //public Category Category { get; set; }
    }
}
