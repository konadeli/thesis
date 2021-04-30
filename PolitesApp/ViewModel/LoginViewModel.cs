﻿using System.ComponentModel.DataAnnotations;

namespace Users.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Να με θυμάσαι/Remember Me")]
        public bool RememberMe { get; set; }
    }
}
