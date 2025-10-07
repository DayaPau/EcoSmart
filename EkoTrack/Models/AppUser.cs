using System.ComponentModel.DataAnnotations;

namespace EkoTrack.Models
{
    public class AppUser
    {
        public int Id { get; set; }

        [Required, EmailAddress, StringLength(160)]
        public string Email { get; set; } = "";

        [Required, StringLength(120)]
        public string FullName { get; set; } = "";

        [Required]
        public string PasswordHash { get; set; } = "";

        [Required]
        public Role Role { get; set; }
    }
}
