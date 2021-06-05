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

            // Query database
            BsonDocument matchThisWeek = new BsonDocument() {{
                "Schema.Workouts.DateCompleted", new BsonDocument() {
                    {"$gte", DateTime.Now.StartOfWeek()},
                    {"$lte", DateTime.Now.EndOfWeek()}
                }
            }};

            BsonDocument matchPending = new BsonDocument() {{
                "Schema.Workouts.DateCompleted", BsonNull.Value
            }};

            BsonDocument match = new BsonDocument() {{
                "$or", new BsonArray { matchThisWeek, matchPending }
            }};

            BsonDocument projection = new BsonDocument() {
                { "Id", "$Schema.Workouts.Id" },
                { "Title", "$Schema.Workouts.Title" },
                { "DateCompleted", "$Schema.Workouts.DateCompleted" }
            };

            List<Workout> workouts = _profiles.Aggregate()
                .Match(new BsonDocument("Username", username))
                .Unwind("Scheme.Workouts")
                .Match(match)
                .Limit(scheme.WorkoutsPerWeek)
                .Project<Workout>(projection)
                .ToList();
            
            // Prepare data
            int completedCount = workouts.Where(w => w.DateCompleted != null).Count();
            float progressPercentage = (completedCount / scheme.WorkoutsPerWeek) * 100;
            Dictionary<int, string> days = new Dictionary<int, string>();

            for(int i = 1; i <= 7; i++) {
                string status = "unavailable";

                // Check if available
                if(profile.Availability.Contains(i)) {
                    status = "available";
                }

                // Check if completed
                Func<Workout, bool> filter = w => {
                    DateTime completed = w.DateCompleted.Value;

                    if(completed != null) {
                        int dayOfWeek = (int) completed.DayOfWeek;
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

        public double GetProcentage(string username) {
            BsonDocument match = new BsonDocument() {{
            "Schema.Workouts.DateAccomplished", new BsonDocument() {
            {"$gte", DateTime.Now.StartOfWeek()},
            {"$lte", DateTime.Now.EndOfWeek()}
    }
}};
            BsonDocument match2 = new BsonDocument() { {
                     "Schema.Workouts.DateAccomplished", new BsonDocument() {
                        { "$eq", null },

                    }
                } };

            var Workoutscountall = _profiles.Aggregate()
                   .Match(new BsonDocument("Username", username))
                   .Unwind("Schema.Workouts").FirstOrDefault().ElementCount;

            var Workoutscountdone = _profiles.Aggregate()
                .Match(new BsonDocument("Username", username))
                .Unwind("Schema.Workouts")
                .Match(match).FirstOrDefault().ElementCount;

            double progresspercentage = Workoutscountdone / Workoutscountall * 100;

            return progresspercentage;
        }

        public int TotalDistance(string username) {
            var Distances = _profiles.Aggregate()
                .Match(new BsonDocument("Username", username))
                .Unwind("Schema.Workouts.distance").ToList();

            int totaldisance = 0;
            foreach(BsonDocument _item in Distances) {
                var item2 = BsonSerializer.Deserialize<int>(_item);
                totaldisance += item2;

            }
            return totaldisance;
        }
        public int TotalWorkouts(string username) {
            var Workoutscountall = _profiles.Aggregate()
                   .Match(new BsonDocument("Username", username))
                   .Unwind("Schema.Workouts").FirstOrDefault().ElementCount;
            return Workoutscountall;
        }
        public double AverageSpeed(string username) {
            var totalspeed = _profiles.Aggregate()
                .Match(new BsonDocument("Username", username))
                .Unwind("Schema.Workouts.Speed").ToList();
            double totalspeedall = 0.0;
            foreach(BsonDocument _item in totalspeed) {
                var item2 = BsonSerializer.Deserialize<double>(_item);
                totalspeedall += item2;
            }

            BsonDocument Workoutdone = new BsonDocument() {{
            "Schema.Workouts.DateAccomplished", new BsonDocument() {
            {"$ne", null}

    }
}};

            var totalspeeddone = _profiles.Aggregate()
                .Match(new BsonDocument("Username", username))
                .Unwind("Schema.Workouts.Speed")
                .Match(Workoutdone).ToList();
            return 0.0;
        }

    }
}



