using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssetTracker.Models
{
    public class AssetRequest
    {
        [Key]
        public int id { get; set; }

        [Required]
        public int user_id { get; set; }

        [ForeignKey("user_id")]
        public User user { get; set; }

        [Required]
        public int asset_id { get; set; }

        [ForeignKey("asset_id")]
        public Asset asset { get; set; }

        [Required]
        [StringLength(20)]
        public string request_type { get; set; } = "Borrow"; // Borrow or Return

        [Required]
        [StringLength(20)]
        public string status { get; set; } = "Pending"; // Pending, Approved, Rejected, Returned

        [Required]
        public DateTime requested_at { get; set; } = DateTime.Now;

        public DateTime? approved_at { get; set; }

        public int? approved_by_admin_id { get; set; }

        [ForeignKey("approved_by_admin_id")]
        public Admin approved_by_admin { get; set; }

        [StringLength(500)]
        public string remarks { get; set; }

        public DateTime? returned_at { get; set; }
    }
}