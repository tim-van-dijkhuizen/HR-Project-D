using System;

namespace FitbyteServer.Models {

    public class Workout {

        public string Id { get; set; }
        public string Title { get; set; }
        public bool Endurance { get; set; }

        public DateTime? DateAccomplished { get; set; }

        public float? Distance { get; set; }
        public int? Time { get; set; }
        public float? Speed { get; set; }
    
    }

}
