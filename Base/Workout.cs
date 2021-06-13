using FitbyteServer.Models;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;

namespace FitbyteServer.Base {

    [BsonDiscriminator(RootClass = true)]
    [BsonKnownTypes(typeof(EnduranceWorkout), typeof(PowerWorkout))]
    public abstract class Workout {

        [BsonIgnore]
        private string _id;

        public string Id { 
            get {
                if(_id == null) {
                    _id = Guid.NewGuid().ToString();
                }

                return _id;
            }
            
            init {
                _id = value;
            }
        }

        [Required]
        [StringLength(300, ErrorMessage = "Title length can't be more than 300")]
        public string Title { get; set; }

        public DateTime? DateCompleted { get; set; }

        public virtual void SetResult(JObject json) { }

    }

}
