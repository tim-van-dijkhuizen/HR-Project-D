using System;
using System.Linq;
using System.Collections.Generic;
using FitbyteServer.Extensions;
using FitbyteServer.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using FitbyteServer.Errors;
using FitbyteServer.Base;

namespace FitbyteServer.Services {

    public class WorkoutService {
        
        private readonly IMongoCollection<Profile> _profiles;
        private readonly ProfileService _profileService;

        public WorkoutService(IMongoDatabase database, ProfileService profileService) {
            _profiles = database.GetCollection<Profile>("profiles");
            _profileService = profileService;
        }
        
        public WeekOverview GetWeekOverview(string username) {
            Profile profile = _profileService.GetProfile(username);

            // Make sure the profile exists
            if(profile == null) {
                throw new ProfileNotFoundException();
            }

            // Make sure the schema exists
            Scheme scheme = profile.Scheme;

            if(scheme == null) {
                throw new SchemeNotFoundException();
            }
            
            // Get workouts
            DateTime now = DateTime.Now;

            List<Workout> workouts = scheme.Workouts.Where(w => {
                DateTime? date = w.DateCompleted;

                if(date != null) {
                    return date >= now.StartOfWeek() && date <= now.EndOfWeek();
                }

                return true;
            })
            .Take(scheme.WorkoutsPerWeek)
            .ToList();

            // Prepare data
            int completedCount = workouts.Where(w => w.DateCompleted != null).Count();
            float progressPercentage = (completedCount / (float) scheme.WorkoutsPerWeek) * 100;
            Dictionary<int, string> days = new Dictionary<int, string>();

            for(int i = 1; i <= 7; i++) {
                string status = "unavailable";

                // Check if available
                if(profile.Availability.Contains(i)) {
                    status = "available";
                }

                // Check if completed
                Func<Workout, bool> filter = w => {
                    DateTime? completed = w.DateCompleted;

                    if(completed != null) {
                        int dayOfWeek = (int) completed.Value.DayOfWeek;
                        return dayOfWeek == i;
                    }

                    return false;
                };

                if(workouts.Where(filter).Any()) {
                    status = "completed";
                }

                days.Add(i, status);
            }

            return new WeekOverview() {
                ProgressPercentage = progressPercentage,
                Days = days,
                Workouts = workouts
            };
        }

        public void CompleteWorkout(string username, string workoutId, int time) {
            BsonDocument filter = new BsonDocument() {
                { "Username", username },
                {  "Scheme.Workouts._id", workoutId }
            };

            // Make sure the workout exists
            if(!_profiles.Find(filter).Any()) {
                throw new WorkoutNotFoundException();
            }

            // Create update
            BsonDateTime dateTime = new BsonDateTime(DateTime.Now);
            BsonDocument result = new BsonDocument() { { "Time", time } };

            BsonDocument update = new BsonDocument() {
                { "$set", new BsonDocument() {
                    { "Scheme.Workouts.$.DateCompleted", dateTime },
                    { "Scheme.Workouts.$.Result", result }
                } }
            };

            // Update workout
            _profiles.UpdateOne(filter, update);
        }

    }

}



