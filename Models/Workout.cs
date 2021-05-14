using System;
using FitbyteServer.Database;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FitbyteServer.Models
{
    public class Workout
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Title { get; set; }

        public bool Endurance { get; set; }


        public int Time { get; set; }

   
        public float Distance { get; set; }

        public float Speed { get; set; }

        public DateTime DateAccomplished { get; set; }

    
    }

}
