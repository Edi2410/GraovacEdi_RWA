﻿using DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace CoreWebApi.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; } = null!;

        public string? Phone { get; set; }

        public bool IsConfirmed { get; set; }

        public string? SecurityToken { get; set; }

        [Required(ErrorMessage = "CountryOfResidenceId is required")]
        public int CountryOfResidenceId { get; set; }

    }
}
