using System.ComponentModel.DataAnnotations;

namespace Javni.Models
{
    public class LoginUser
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Confirm password is required")]
        [Compare("Password")]
        public string? ConfirmPassword { get; set; } = null!;
    }
}
