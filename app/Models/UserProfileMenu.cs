using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssetTracker.Models
{
    [Table("user_profile_menus")]
    public class UserProfileMenu
    {
        [Key]
        public int id { get; set; }

        [Required]
        public int user_profile_id { get; set; }

        [Required]
        public int menu_id { get; set; }

        public bool can_view { get; set; } = true;

        public int status { get; set; } = 1;

        public DateTime created_at { get; set; } = DateTime.Now;

        public DateTime updated_at { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("user_profile_id")]
        public UserProfile UserProfile { get; set; }

        [ForeignKey("menu_id")]
        public Menu Menu { get; set; }
    }
}