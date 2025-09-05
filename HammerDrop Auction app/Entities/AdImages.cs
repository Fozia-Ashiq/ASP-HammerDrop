using System.ComponentModel.DataAnnotations;

namespace HammerDrop_Auction_app.Entities
{
    public class AdImages
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Image path is required")]
        public string ImageName { get; set; }

        [Required]
        public int AdId { get; set; } // FK matching navigation property

        public Ad Ad { get; set; } // Navigation property
    }
}
