using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace swe_2_project.Models
{
	public enum user_status
	{
		active,
		inactive
	}

	[Table("users")]
	public class users
	{
        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string password_hash { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public DateOnly dob { get; set; }
        public string? verification_token { get; set; }
        public DateTime? verified_at { get; set; }
        public user_status status { get; set; }
    }
}

