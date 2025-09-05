using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HammerDrop_Auction_app.Entities
{
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    public class UserAccount
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string FirstName { get; set; }

        [Required, MaxLength(50)]
        public string LastName { get; set; }

        [Required, MaxLength(100)]
        public string Email { get; set; }

        [Required, MaxLength(30)]
        public string UserName { get; set; }

        [Required, MaxLength(20)]
        public string Password { get; set; }

        // New: Store verification code in DB
        [MaxLength(10)]
        public string VerificationCode { get; set; }

        public bool IsEmailVerified { get; set; } = false; // Optional: add verification flag
    }
}
