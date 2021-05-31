using System.ComponentModel.DataAnnotations;

namespace FitbyteServer.Models {

    public class FitbitToken {

        [Required]
        public string AccessToken { get; set; }

        [Required]
        public string RefreshToken { get; set; }

    }

}
