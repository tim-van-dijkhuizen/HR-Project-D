namespace FitbyteServer.Models {

    public class Progress {

        public float TotalPercentage { get; set; }

        public int TotalDistance { get; set; }
        public int TotalWorkouts { get; set; }

        public float AverageSpeed { get; set; }
        public float MaxSpeed { get; set; }

        public int AverageDistance { get; set; }
        public int MaxDistance { get; set; }

    }

}
