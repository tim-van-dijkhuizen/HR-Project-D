using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace FitbyteServer.Models {

    public class Workout {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public bool Endurance { get; set; }

        public DateTime? DateAccomplished { get; set; }

        public float? Distance { get; set; }
        public int? Time { get; set; }
        public float? Speed { get; set; }
    
    }

}
