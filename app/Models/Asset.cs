using System;
using System.ComponentModel.DataAnnotations;
namespace AssetTracker.Models
{
	public class Asset
	{

		[Key]
        public int id { get; set; }

		[Required]
		public string assetName { get; set; }

		[Required]
		public int Quantity { get; set; } = 0;

		[Required]
		public string AssignedTo { get; set; }

		[Required]
		public string Category { get; set; }

	}
}

