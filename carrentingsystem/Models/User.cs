using System.ComponentModel.DataAnnotations;

namespace carrentingsystem.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Name { get; set; }
        public bool IsAdmin { get; set; }
        public RoleType Role { get; set; }
    }
    public enum RoleType {  Admin = 1 , Customer = 2}
}
