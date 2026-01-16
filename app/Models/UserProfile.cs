
using System.ComponentModel.DataAnnotations;

namespace AssetTracker.Models
{
	public class UserProfile
	{
        [Key]
        public int id { get; set; }

        public string profile_name { get; set; }
       
    }
}

