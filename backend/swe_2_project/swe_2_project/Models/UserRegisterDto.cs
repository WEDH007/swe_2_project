using System;
namespace swe_2_project.Models
{
	public class UserRegisterDto
	{
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateOnly Dob { get; set; }
    }
}

