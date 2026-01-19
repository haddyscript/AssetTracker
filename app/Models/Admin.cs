using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssetTracker.Models
{
    [Table("admins")]
    public class Admin
    {
        [Key]
        public int id { get; set; }

        [Required]
        [StringLength(100)]
        public string username { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string email { get; set; }

        [Required]
        [StringLength(255)]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        public string password_hash { get; set; }

        [Required]
        [StringLength(150)]
        public string full_name { get; set; }

        public bool is_active { get; set; } = true;

        public DateTime created_at { get; set; } = DateTime.Now;

        public DateTime updated_at { get; set; } = DateTime.Now;

        // -------------------------------
        // user profile foreign key

        [Required]
        [ForeignKey("UserProfile")]
        public int? user_profile { get; set; }

        public UserProfile UserProfile { get; set; }
    }
}
