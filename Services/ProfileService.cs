using MongoDB.Driver;
using FitbyteServer.Models;
using System;
using System.Collections.Generic;
using FitbyteServer.Base;
using FitbyteServer.Helpers;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
            var today = DateTime.Today;
            // Calculate the age.
            int age = today.Year - dateOfBirth.Year;
            string test = "male.json";
            if(gender == Genders.F)
            {
                test = "female.json";
            }
            StreamReader r = new StreamReader(test);
            string jsonString = r.ReadToEnd();
            JObject JsonCondition = JObject.Parse(jsonString);
            List<Dictionary<string, object>> condition = JsonCondition.ToObject<List<Dictionary<string, object>>>();
          
            foreach(var con in condition)
            {
                if(int.Parse(con["minAge"].ToString()) >= age && int.Parse(con["maxAge"].ToString()) <= age)
                {
                    if(int.Parse(con["minDistance"].ToString()) >= distance && int.Parse(con["maxDistance"].ToString()) <= distance)
                    {
                        return (ConditionScores)Enum.Parse(typeof(ConditionScores), con["score"].ToString());
                    }
                    
                }
            }
            return ConditionScores.None;

        }

        public Scheme GenerateScheme(Goals distanceGoal, int timeGoal, int daysAvailable, ConditionScores score) {
            //StreamReader r = new StreamReader(workouts5kmPlus.json);
            //string jsonString = r.ReadToEnd();
            //JObject JsonCondition = JObject.Parse(jsonString);



            Scheme schema = new Scheme();

            schema.ConditionScore = score;
            schema.WorkoutsPerWeek = 3;

            schema.Workouts = new List<Workout>();

            
            schema.Workouts.Add(new EnduranceWorkout() { Title = $"Hardlopen 3,2km", Distance = 3.20f, Time = (timeGoal / 5) + 3 * 3 });
            schema.Workouts.Add(new PowerWorkout() { Title = $"Krachttraining, 25 min" });


            

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
