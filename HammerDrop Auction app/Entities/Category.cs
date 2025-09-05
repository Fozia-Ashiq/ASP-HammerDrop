using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HammerDrop_Auction_app.Entities;


public class Category
{
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters")]
        public string Title { get; set; }
        public ICollection<Subcategory> Subcategories { get; set; }

        //[Required(ErrorMessage = "Description is required")]
        //[StringLength(1000, ErrorMessage = "Description can't exceed 1000 characters")]
       // public string Description { get; set; }
        public string ImageName { get; set; }

        [NotMapped]
        public IFormFile Image { get; set; }
}

