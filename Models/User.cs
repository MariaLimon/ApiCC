using System.ComponentModel.DataAnnotations;

namespace ApiCC.models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public DateTime DateRegister { get; set; } = DateTime.Now;
        public bool IsEmailConfirmed { get; set; }
        public bool IsAdmin { get; set; }
    }
}