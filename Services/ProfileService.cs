using MongoDB.Driver;
using FitbyteServer.Extensions;
using FitbyteServer.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using FitbyteServer.Base;

namespace FitbyteServer.Services {

    public class ProfileService {

        private readonly IMongoCollection<Profile> _profiles;

        public ProfileService(IMongoDatabase database) {
            _profiles = database.GetCollection<Profile>("profiles");
        }

        public Profile GetProfile(string username) {
            return _profiles.Find<Profile>(profile => profile.Username == username.ToLower())
                .FirstOrDefault();
        }

        public void SaveProfile(Profile profile) {
            string username = profile.Username.ToLower();
            Profile existing = GetProfile(username);

            // Create if its a new profile
            if(existing == null) {
                _profiles.InsertOne(profile);
                return;
            }

            // Update existing profile
            _profiles.ReplaceOne(p => p.Username == username, profile);
        }

        public ConditionScores GetConditionScore(Genders gender, DateTime dateOfBirth, float distance) {
            return ConditionScores.Medium;
        }

        public Schema GenerateSchema(Goals goal, ConditionScores score) {
            Schema schema = new Schema();

            schema.ConditionScore = score;
            schema.WorkoutsPerWeek = 3;

            schema.Workouts = new List<Workout>();
            schema.Workouts.Add(new Workout() { Title = "Hardlopen 5km", Endurance = true, Distance = 3.2f });
            schema.Workouts.Add(new Workout() { Title = "Hardlopen 10km", Endurance = true, Distance = 3.2f });
            schema.Workouts.Add(new Workout() { Title = "Opdrukken 100x", Endurance = false });

            schema.Progress = new Progress();

            return schema;
        }

    }

}
