using System.ComponentModel.DataAnnotations;

namespace FitbyteServer.Models {

    public class Progress {

        [Required]
        [Range(0, float.MaxValue, ErrorMessage = "TotalPercentage must be positive")]
        public float TotalPercentage { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "TotalDistance must be positive")]
        public int TotalDistance { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "TotalWorkouts must be positive")]
        public int TotalWorkouts { get; set; }

        [Required]
        [Range(0, float.MaxValue, ErrorMessage = "AverageSpeed must be positive")]
        public float AverageSpeed { get; set; }

        [Required]
        [Range(0, float.MaxValue, ErrorMessage = "MaxSpeed must be positive")]
        public float MaxSpeed { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "AverageDistance must be positive")]
        public int AverageDistance { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "MaxDistance must be positive")]
        public int MaxDistance { get; set; }

    }

}
