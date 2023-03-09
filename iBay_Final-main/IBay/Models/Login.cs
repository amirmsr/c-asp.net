using System;
using System.ComponentModel.DataAnnotations;

namespace IBay.Models
{
    public class Login
    {
        [Required]
        public string Pseudo { get; set; }

        [Required]
        public string Password { get; set; }
    }
}

