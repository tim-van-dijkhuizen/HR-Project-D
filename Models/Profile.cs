using FitbyteServer.Base;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FitbyteServer.Models {

    public class Profile {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        public Genders Gender { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        public Goals DistanceGoal { get; set; }

        [Required]
        public int TimeGoal { get; set; }

        [Required]
        public List<int> Availability { get; set; }

        public FitbitToken FitbitToken { get; set; }

        public Scheme Scheme { get; set; }

    }

}
