using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssetTracker.Models
{
    [Table("user_profile_permissions")]
    public class UserProfilePermission
    {
        [Key]
        public int id { get; set; }

        [Required]
        public int user_profile_id { get; set; }

        [Required]
        [StringLength(100)]
        public string module_name { get; set; }

        public bool can_view { get; set; } = false;
        public bool can_create { get; set; } = false;
        public bool can_edit { get; set; } = false;
        public bool can_delete { get; set; } = false;

        public int status { get; set; } = 1;

        public DateTime created_at { get; set; } = DateTime.Now;
        public DateTime updated_at { get; set; } = DateTime.Now;

        // Navigation property
        [ForeignKey("user_profile_id")]
        public UserProfile UserProfile { get; set; }
    }
}
