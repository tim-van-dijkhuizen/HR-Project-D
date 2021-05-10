using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace FitbyteServer.Base {

    public class Profile {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Username { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Genders Gender { get; set; }

        public DateTime DateOfBirth { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Goals Goal { get; set; }

        public List<int> Availability { get; set; }

    }

}
