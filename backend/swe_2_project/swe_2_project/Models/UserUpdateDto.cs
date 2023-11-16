using System;
namespace swe_2_project.Models
{
    public class UserUpdateDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateOnly Dob { get; set; }
    }
}

