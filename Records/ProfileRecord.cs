using FitbyteServer.Base;
using FitbyteServer.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace FitbyteServer.Records {

    public class ProfileRecord {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        public string Username { get; set; }
        
        public Genders Gender { get; set; }
        
        public DateTime DateOfBirth { get; set; }
        
        public Goals DistanceGoal { get; set; }
        
        public int TimeGoal { get; set; }
        
        public List<int> Availability { get; set; }

        public FitbitToken FitbitToken { get; set; }

        public Scheme Scheme { get; set; }

    }

}
