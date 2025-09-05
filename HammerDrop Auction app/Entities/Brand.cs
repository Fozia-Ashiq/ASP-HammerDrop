using System.ComponentModel.DataAnnotations;

namespace HammerDrop_Auction_app.Entities
{
    public class Brand
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters")]
        public string Title { get; set; }
        public int SubcategoryId { get; set; }
        public Subcategory Subcategory { get; set; }
    }
}
