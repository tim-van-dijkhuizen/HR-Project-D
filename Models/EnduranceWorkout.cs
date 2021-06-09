using FitbyteServer.Base;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;

namespace FitbyteServer.Models {

    public class EnduranceWorkout : Workout {

        [Required]
        public int Distance { get; set; }

        [Required]
        public int Time { get; set; }

        public EnduranceResult Result { get; set; }

        public override void SetResult(JObject json) {
            Result = new EnduranceResult();

            // Set distance
            JToken distanceProperty = json.GetValue("distance");
            int distance = Result.Distance = distanceProperty.Value<int>();

            // Set time
            JToken timeProperty = json.GetValue("time");
            int time = Result.Time = timeProperty.Value<int>();
            
            // Set speed
            Result.Speed = (float) distance / time * 3.6f;
        }
    
    }

}
