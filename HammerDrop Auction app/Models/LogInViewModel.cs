using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HammerDrop_Auction_app.Models
{
    public class LogInViewModel
    {
        [Required(ErrorMessage = "UserName or Email is required")]
        [MaxLength(30, ErrorMessage = "Username cannot exceed 30 characters")]
        [DisplayName("Username or Email")]
        public string UserNameorEmail { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Password cannot exceed 20 or minimum 5 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
