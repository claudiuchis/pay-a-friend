using System;
using System.ComponentModel.DataAnnotations;

namespace App.Identity.Authentication
{
    public class LoginInputModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}