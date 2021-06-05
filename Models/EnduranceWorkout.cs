using FitbyteServer.Base;
using System.ComponentModel.DataAnnotations;

namespace FitbyteServer.Models {

    public class EnduranceWorkout : Workout {

        [Required]
        public float Distance { get; set; }

        [Required]
        public int Time { get; set; }

        public EnduranceResult Result { get; set; }
    
    }

}
