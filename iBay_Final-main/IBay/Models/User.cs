using System;
using System.ComponentModel.DataAnnotations;

namespace IBay.Models
{
	public class User
	{
        public int Id { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Pseudo is required.")]
        public string Pseudo { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        [RegularExpression("^(admin|user|seller)$", ErrorMessage = "Role must be 'admin', 'user' or 'seller'.")]
        public string Role { get; set; }
    }
}

