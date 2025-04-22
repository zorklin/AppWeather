using System.ComponentModel.DataAnnotations;

namespace AppWeather.Models
{
    public class Admin
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [MaxLength(50, ErrorMessage = "Username cannot exceed 50 characters")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MaxLength(50, ErrorMessage = "Password cannot exceed 50 characters")]
        public string? Password { get; set; }
    }
}