using FitbyteServer.Base;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FitbyteServer.Models {

    public class Scheme {

        [Required]
        public ConditionScores ConditionScore;

        [BsonIgnore]
        private List<Workout> _workouts;

        [Required]
        public List<Workout> Workouts {
            get {
                if(_workouts == null) {
                    return _workouts = new List<Workout>();
                }

                return _workouts;
            }
            init {
                _workouts = value;
            }
        }


        [BsonIgnore]
        private Progress _progress;

        [Required]
        public Progress Progress {
            get {
                if(_progress == null) {
                    return _progress = new Progress();
                }

                return _progress;
            }
            set {
                _progress = value;
            }    
        }

    }

}
