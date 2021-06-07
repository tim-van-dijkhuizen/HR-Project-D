namespace FitbyteServer.Models {

    public class Progress {

        public float TotalPercentage { get; set; }

        public int TotalDistance { get; set; }
        public int TotalWorkouts { get; set; }

        public float AverageSpeed { get; set; }
        public float MaxSpeed { get; set; }

        public float AverageDistance { get; set; }
        public float MaxDistance { get; set; }

    }

}
