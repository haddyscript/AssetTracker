using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssetTracker.Models
{
    [Table("assets")]
    public class Asset
    {
        [Key]
        public int id { get; set; }

        [Required]
        [StringLength(100)]
        public string asset_tag { get; set; }

        [Required]
        [StringLength(150)]
        public string asset_name { get; set; }

        [StringLength(500)]
        public string description { get; set; }

        [StringLength(100)]
        public string category { get; set; }

        [StringLength(100)]
        public string brand { get; set; }

        [StringLength(100)]
        public string model { get; set; }

        [StringLength(150)]
        public string serial_number { get; set; }

        [DataType(DataType.Date)]
        public DateTime? purchase_date { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? purchase_price { get; set; }

        [Required]
        [StringLength(50)]
        public string status { get; set; } = "Available";

        [StringLength(50)]
        public string condition { get; set; }

        public int ?assigned_to_user_id { get; set; }

        [ForeignKey("assigned_to_user_id")]
        public User ?assigned_user { get; set; }

        public DateTime? assigned_date { get; set; }

        public DateTime created_at { get; set; } = DateTime.Now;

        public DateTime updated_at { get; set; } = DateTime.Now;
    }
}
