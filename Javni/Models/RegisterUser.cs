using System.ComponentModel.DataAnnotations;

namespace Javni.Models
{
    public class RegisterUser
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Confirm password is required")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = null!;

        [Required(ErrorMessage = "Firsst name is required")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Email password is required")]
        public string Email { get; set; } = null!;

        public string? Phone { get; set; } = null!;

        [Display(Name = "Country of residence")]
        public int CountryOfResidenceId { get; set; }
    }
}
