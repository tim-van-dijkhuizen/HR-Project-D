using System;
using System.Collections.Generic;
using FitbyteServer.Extensions;
using FitbyteServer.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace FitbyteServer.Services
{
    public class WorkoutService
    {


        private readonly IMongoCollection<Profile> _profiles;

        public WorkoutService(IMongoDatabase database)
        {
            _profiles = database.GetCollection<Profile>("profiles");
        }

        public bool CreateWorkout(Workout workout)
        {
            try
            {
                //.InsertOne(workout);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public List<Workout> Weekoverview(string username)
        {
            BsonDocument match = new BsonDocument() {{
            "Schema.Workouts.DateAccomplished", new BsonDocument() {
            {"$gte", DateTime.Now.StartOfWeek()},
            {"$lte", DateTime.Now.EndOfWeek()}
    }
}};
            BsonDocument match2 = new BsonDocument() { {
                     "Schema.Workouts.DateAccomplished", BsonNull.Value
                }
                };

            List<BsonDocument> result = _profiles.Aggregate()
                .Match(new BsonDocument("Username", username))
                .Unwind("Schema.Workouts")
                .Match(match).ToList();


            List<BsonDocument> WeekWorkout = _profiles.Aggregate()
                   .Match(new BsonDocument("Username", username))
                   .Unwind("Schema.Workouts")
                   .Match(match2)
                   .Project(new BsonDocument() {
                       { "Id", "$Schema.Workout.Id" },
                        { "Title", "$Schema.Workout.Title" },
                        { "Endurance", "$Schema.Workout.Endurance" },
                        { "Time", "$Schema.Workout.Time" },
                        { "Distance", "$Schema.Workout.Distance" },
                       { "Speed", "$Schema.Workout.Speed"},
                       {"DateCompleted", "$Schema.Workout.DateCompleted" }
                    })
                   .ToList();

            List<Workout> weekworkoutsresult = new List<Workout>();


            foreach (BsonDocument _item in WeekWorkout)
            {
                var item2 =
                BsonSerializer.Deserialize<Workout>(_item);
                weekworkoutsresult.Add(item2);

            }

            return weekworkoutsresult;
        }
        public double GetWeekProcentage(string username)
        {
            BsonDocument match = new BsonDocument() {{
            "Schema.Workouts.DateAccomplished", new BsonDocument() {
            {"$gte", DateTime.Now.StartOfWeek()},
            {"$lte", DateTime.Now.EndOfWeek()}
    }
}};
            BsonDocument match2 = new BsonDocument() { {
                     "Schema.Workouts.DateAccomplished", BsonNull.Value


                } };


            var Workoutscountdone = _profiles.Aggregate()
                .Match(new BsonDocument("Username", username))
                .Unwind("Schema.Workouts")
                .Match(match).FirstOrDefault();

            //.ElementCount;

            double progresspercentage = (Workoutscountdone != null ? Workoutscountdone.ElementCount : 0) / 4 * 100;

            return progresspercentage;
        }

        public List<int> GetWeekDays(string username)
        {

            List<BsonDocument> resultDays = _profiles.Aggregate()
                .Match(new BsonDocument("Username", username))
                .Project(new BsonDocument() { { "Availability", $"Schema.Availability" } }).ToList();



            var match = _profiles.Find(new BsonDocument() { { "Username", username } })
                .Project(new BsonDocument() {
                       { "Availability", true } }).FirstOrDefault();
       
        


            BsonArray avaliabilityresult = match.GetValue("Availability").AsBsonArray;
            List<int> test = new List<int>();
          
            foreach (BsonValue _item in avaliabilityresult)
            
             {
                var item2 = BsonSerializer.Deserialize<int>(_item);
                test.Add(item2);

             }

            return test;
        }

        public double GetProcentage(string username)
        {
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

        public int TotalDistance(string username)
        {
            var Distances = _profiles.Aggregate()
                .Match(new BsonDocument("Username", username))
                .Unwind("Schema.Workouts.distance").ToList();

            int totaldisance = 0;
            foreach (BsonDocument _item in Distances)
            {
                var item2 = BsonSerializer.Deserialize<int>(_item);
                totaldisance += item2;

            }
            return totaldisance;
        }
        public int TotalWorkouts(string username)
        {
            var Workoutscountall = _profiles.Aggregate()
                   .Match(new BsonDocument("Username", username))
                   .Unwind("Schema.Workouts").FirstOrDefault().ElementCount;
            return Workoutscountall;
        }
        public double AverageSpeed(string username)
        {
            var totalspeed = _profiles.Aggregate()
                .Match(new BsonDocument("Username", username))
                .Unwind("Schema.Workouts.Speed").ToList();
            double totalspeedall = 0.0;
            foreach (BsonDocument _item in totalspeed)
            {
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
    

        public void CompleteWorkout()
        {
            DateTime today = DateTime.Today;

        }
    }
}



