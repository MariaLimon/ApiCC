using System.ComponentModel.DataAnnotations;

namespace ApiCC.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public bool IsEmailConfirmed { get; set; }
        public bool IsAdmin { get; set; }
    }
}