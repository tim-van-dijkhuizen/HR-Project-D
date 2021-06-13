using MongoDB.Driver;
using FitbyteServer.Models;
using System;
using System.Collections.Generic;
using FitbyteServer.Base;
using FitbyteServer.Helpers;
using FitbyteServer.Records;

namespace FitbyteServer.Services {

    public class ProfileService {

        private readonly IMongoCollection<ProfileRecord> _profiles;

        public ProfileService(IMongoDatabase database) {
            _profiles = database.GetCollection<ProfileRecord>("profiles");
        }

        public Profile GetProfile(string username) {
            ProfileRecord record = _profiles.Find(profile => profile.Username == username).FirstOrDefault();

            if(record != null) {
                return DatabaseHelper.ParseProfile(record);
            }

            return null;
        }

        public bool DoesProfileExist(string username) {
            return _profiles.Find(profile => profile.Username == username).Any();
        }

        public void SaveProfile(string username, Profile profile) {
            ProfileRecord record = DatabaseHelper.PrepareProfile(username, profile);

            if(DoesProfileExist(username)) {
                _profiles.ReplaceOne(p => p.Username == username, record);
            } else {
                _profiles.InsertOne(record);
            }
        }

        public ConditionScores GetConditionScore(Genders gender, DateTime dateOfBirth, float distance) {
            return ConditionScores.Medium;
        }

        public Scheme GenerateScheme(Goals distanceGoal, int timeGoal, int daysAvailable, ConditionScores score) {
            Scheme schema = new() {
                ConditionScore = score,
                WorkoutsPerWeek = 3,
                Workouts = new List<Workout>()
            };

            for(int i = 1; i <= 20; i++) {
                schema.Workouts.Add(new EnduranceWorkout() {
                    Title = $"Hardlopen {i}km",
                    Distance = i
                });
            }

            schema.Workouts.Add(new PowerWorkout() {
                Title = "Opdrukken 100x"
            });

            schema.Progress = new Progress();

            return schema;
        }

    }

}
