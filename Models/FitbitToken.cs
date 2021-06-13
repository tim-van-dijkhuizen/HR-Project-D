using System.ComponentModel.DataAnnotations;

namespace FitbyteServer.Models {

    public class FitbitToken {

        [Required]
        [StringLength(300, ErrorMessage = "AccessToken length can't be more than 300")]
        public string AccessToken { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "RefreshToken length can't be more than 100")]
        public string RefreshToken { get; set; }

    }

}
