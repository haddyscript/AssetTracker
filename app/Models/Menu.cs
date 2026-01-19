using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AssetTracker.Models
{
    [Table("menus")]
    public class Menu
    {
        [Key]
        public int id { get; set; }

        [Required]
        [StringLength(100)]
        public string menu_name { get; set; }

        [Required]
        [StringLength(150)]
        public string route { get; set; }

        [StringLength(50)]
        public string? icon { get; set; }

        public int? parent_id { get; set; }

        public int sort_order { get; set; } = 0;

        public bool is_active { get; set; } = true;

        public DateTime created_at { get; set; } = DateTime.Now;

        public DateTime updated_at { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("parent_id")]
        [BindNever]
        public Menu? ParentMenu { get; set; }

        [BindNever]
        public ICollection<Menu>? ChildMenus { get; set; }
    }
}