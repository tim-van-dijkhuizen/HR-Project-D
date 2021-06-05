using FitbyteServer.Base;
using System.Collections.Generic;

namespace FitbyteServer.Models {

    public class WeekOverview {

        public float ProgressPercentage { get; set; }
        public Dictionary<int, string> Days { get; set; }
        public List<Workout> Workouts { get; set; }

    }

}
