using MongoDB.Driver;
using FitbyteServer.Models;
using System;
using System.Collections.Generic;
using FitbyteServer.Base;
using FitbyteServer.Helpers;

namespace FitbyteServer.Services {

    public class ProfileService {

        private readonly IMongoCollection<Profile> _profiles;

        public ProfileService(IMongoDatabase database) {
            _profiles = database.GetCollection<Profile>("profiles");
        }

        public Profile GetProfile(string username) {
            username = ProfileHelper.ParseUsername(username);
            return _profiles.Find(profile => profile.Username == username).FirstOrDefault();
        }

        public void SaveProfile(Profile profile) {
            Profile existing = GetProfile(profile.Username);

            // Create or update profile
            if(existing != null) {
                _profiles.ReplaceOne(p => p.Username == existing.Username, profile);
            } else {
                _profiles.InsertOne(profile);
            }
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
