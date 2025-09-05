using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HammerDrop_Auction_app.Entities
{
   public class Subcategory
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters")]
        public string Title { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public string ImageName { get; set; }

        [NotMapped]
        public IFormFile Image { get; set; }

        public ICollection<Product> Products { get; set; }
    }

}
