using FitbyteServer.Base;
using System.Collections.Generic;

namespace FitbyteServer.Models {

    public class Scheme {

        public ConditionScores ConditionScore;
        public int WorkoutsPerWeek { get; set; }
        public List<Workout> Workouts { get; set; }
        public Progress Progress { get; set; }

    }

}
