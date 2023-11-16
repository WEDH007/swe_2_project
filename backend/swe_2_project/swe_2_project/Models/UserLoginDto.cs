using System;
using System.ComponentModel.DataAnnotations;

namespace swe_2_project.Models
{
	public class UserLoginDto
	{
        [Required(ErrorMessage = "Email is required")]
        public string email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string password { get; set; }
    }
}

