using System.ComponentModel.DataAnnotations;

public class ProductImageUploadViewModel

{
    [Required]
    public int ProductId { get; set; }

    [Required(ErrorMessage = "Please upload at least one image")]
    public List<IFormFile> ImageFiles { get; set; }
}
