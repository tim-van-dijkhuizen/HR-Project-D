using FitbyteServer.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FitbyteServer.Models {

    public class Scheme {

        [Required]
        public ConditionScores ConditionScore;

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "WorkoutsPerWeek must be positive")]
        public int WorkoutsPerWeek { get; set; }

        [Required]
        public List<Workout> Workouts { get; set; }

        [Required]
        public Progress Progress { get; set; }

    }

}
