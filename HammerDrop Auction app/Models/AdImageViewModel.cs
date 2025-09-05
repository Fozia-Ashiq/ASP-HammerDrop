using System.ComponentModel.DataAnnotations;

namespace HammerDrop_Auction_app.Models


{
    public class AdImageViewModel
    {
         [Required]
    public int AdId { get; set; }

    [Required(ErrorMessage = "Please upload at least one image")]
    public List<IFormFile> ImageFiles { get; set; }
    }
}
