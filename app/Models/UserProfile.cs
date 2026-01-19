using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssetTracker.Models
{
	[Table("user_profile")]
	public class UserProfile
	{
        [Key]
        public int id { get; set; }

        public string profile_name { get; set; }
       
    }
}

