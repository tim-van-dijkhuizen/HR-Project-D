namespace FitbyteServer.Models {

    public class Progress {

        public float totalPercentage { get; set; }

        public int totalDistance { get; set; }
        public int totalWorkouts { get; set; }

        public float averageSpeed { get; set; }
        public float maxSpeed { get; set; }

        public float averageDistance { get; set; }
        public float maxDistance { get; set; }

    }

}
