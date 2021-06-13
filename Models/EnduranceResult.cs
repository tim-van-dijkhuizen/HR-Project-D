using System.ComponentModel.DataAnnotations;

namespace FitbyteServer.Models {

    public class EnduranceResult {

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Distance must be one or higher")]
        public int Distance { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Time must be one or higher")]
        public int Time { get; set; }
        
        [Required]
        [Range(1, float.MaxValue, ErrorMessage = "Speed must be positive")]
        public float Speed { get; set; }

    }

}
