using FitbyteServer.Base;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;

namespace FitbyteServer.Models {

    public class EnduranceWorkout : Workout {

        [Required]
        public float Distance { get; set; }

        [Required]
        public int Time { get; set; }

        public EnduranceResult Result { get; set; }

        public override void SetResult(JObject json) {
            Result = new EnduranceResult();

            // Set time
            JToken property = json.GetValue("time");
            int time = property.Value<int>();
            
            Result.Time = time;
        }
    
    }

}
