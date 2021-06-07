﻿using MongoDB.Driver;
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

        public bool DoesProfileExist(string username) {
            username = ProfileHelper.ParseUsername(username);
            return _profiles.Find(profile => profile.Username == username).Any();
        }

        public void SaveProfile(Profile profile) {
            string username = ProfileHelper.ParseUsername(profile.Username);

            // Create or update profile
            if(DoesProfileExist(username)) {
                _profiles.ReplaceOne(p => p.Username == username, profile);
            } else {
                _profiles.InsertOne(profile);
            }
        }

        public ConditionScores GetConditionScore(Genders gender, DateTime dateOfBirth, float distance) {
            return ConditionScores.Medium;
        }

        public Scheme GenerateScheme(Goals distanceGoal, int timeGoal, int daysAvailable, ConditionScores score) {
            Scheme schema = new Scheme {
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
