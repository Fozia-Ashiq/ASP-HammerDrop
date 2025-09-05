using System.ComponentModel.DataAnnotations;
namespace HammerDrop_Auction_app.Entities
{
    public class ProductImage
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Image path is required")]
        public string ImageName { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }

}
