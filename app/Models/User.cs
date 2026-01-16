using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssetTracker.Models
{
	public class User
	{
        [Key]
        public int id { get; set; }

        [Required(ErrorMessage = "Username is required") ]
        public string username { get; set; }

        [Required(ErrorMessage = "Full name is required")]
        public string full_name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string email { get; set; }

        public DateTime created_at { get; set; } = DateTime.Now;

        public DateTime updated_at { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        public string password { get; set; }


        // -------------------------------
        // user profile foreign key

        [Required]
        [ForeignKey("UserProfile")]
        public int ?user_profile { get; set; }

        public UserProfile UserProfile { get; set; }

    }
}

