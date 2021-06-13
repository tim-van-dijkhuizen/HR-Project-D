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
            var today = DateTime.Now;
            // Calculate the age.
            int age = today.Year - dateOfBirth.Year;
            string test = "male.json";
            if(gender == Genders.F)
            {
                test = "female.json";
            }
            StreamReader r = new StreamReader("Models/Json/" + test);
            string jsonString = r.ReadToEnd();
            JArray JsonCondition = JArray.Parse(jsonString);
            List<Dictionary<string, object>> condition = JsonCondition.ToObject<List<Dictionary<string, object>>>();
          
            for(int i = 0; i < condition.Count; i++)
            {
                var con = condition[i];

                if (Convert.ToInt32(con["minAge"]) <= age && Convert.ToInt32(con["maxAge"]) >= age)
                {
                    if (Convert.ToInt32(condition[i]["minDistance"]) <= distance && Convert.ToInt32(condition[i]["maxDistance"]) >= distance)
                    {
                        return (ConditionScores)Enum.Parse(typeof(ConditionScores), con["score"].ToString());
                    }

                }
            }
            return ConditionScores.None;

        }

        public Scheme Generate(int timeGoal, int daysAvailable, ConditionScores score, string workout)
        {
            Scheme schemas = new Scheme();
            schemas.Workouts = new List<Workout>();
            schemas.WorkoutsPerWeek = daysAvailable;
            schemas.ConditionScore = score;
            workout = "Models/Json/" + workout;
            StreamReader r = new StreamReader(workout);
            string jsonString = r.ReadToEnd();
            JArray JsonCondition = JArray.Parse(jsonString);
            List<Dictionary<string, object>> condition = JsonCondition.ToObject<List<Dictionary<string, object>>>();

            for (int i = 0; i < condition.Count; i++)
            {
                var con = condition[i];

                if ((string)con["type"] == "Endurance")
                {
                    int tijd = Convert.ToInt32(con["tijd"]);
                    int MintimeGoal = timeGoal / 60;
                    int tijden = MintimeGoal / 5 * Convert.ToInt32(condition[i]["afstand"]) / 1000 + tijd;
                    tijden = tijden * 60;
                    schemas.Workouts.Add(new EnduranceWorkout() { Title = (string)con["title"], Distance = Convert.ToInt32((con["afstand"])), Time = tijden });

                }
                else
                {
                    schemas.Workouts.Add(new PowerWorkout() { Title = (string)con["title"] });
                }



            }
            return schemas;

        }

        public Scheme GenerateScheme(Goals distanceGoal, int timeGoal, int daysAvailable, ConditionScores score) {
           
            //Scheme schemas = new Scheme();
            //schemas.Workouts = new List<Workout>();
            //schemas.WorkoutsPerWeek = daysAvailable;
            //schemas.ConditionScore = score;
            //string workout = "Models/Json/";
            if ((score == ConditionScores.Low || score == ConditionScores.Lowest) && distanceGoal == Goals.Km5)
            {
                return Generate(timeGoal, daysAvailable, score, "km5Min.json");


            }
      
            if ((score == ConditionScores.High || score == ConditionScores.Highest) && distanceGoal == Goals.Km5)
            {
                return Generate(timeGoal, daysAvailable, score, "workouts5kmPlus.json");
            }
           
            if (score == ConditionScores.Low && distanceGoal == Goals.Km10)
            {
                return Generate(timeGoal, daysAvailable, score, "10kmMin.json");
            }
            if (score == ConditionScores.High && distanceGoal == Goals.Km10)
            {
                return Generate(timeGoal, daysAvailable, score, "10km.json");
            }
            return new Scheme();
            

            //if (score == ConditionScores.Low && distanceGoal == Goals.Km21)
            //{
            //    StreamReader r = new StreamReader("Models/Json/workouts5kmPlus.json");
            //    string jsonString = r.ReadToEnd();
            //    JArray JsonCondition = JArray.Parse(jsonString);
            //    List<Dictionary<string, object>> condition = JsonCondition.ToObject<List<Dictionary<string, object>>>();
            //    for (int i = 0; i < condition.Count; i++)
            //    {
            //        var con = condition[i];

            //        if ((string)con["type"] == "Endurance")
            //        {
            //            int tijd = Convert.ToInt32(con["tijd"]);
            //            int tijden = (timeGoal / 5) * (Convert.ToInt32(condition[i]["afstand"]) + tijd);
            //            schemas.Workouts.Add(new EnduranceWorkout() { Title = (string)con["title"], Distance = Convert.ToInt32((con["afstand"])), Time = tijden });

            //        }
            //        else
            //        {
            //            schemas.Workouts.Add(new PowerWorkout() { Title = (string)con["title"] });
            //        }



            //    }
            //    return schemas;

            //}
            //if (score == ConditionScores.Low && distanceGoal == Goals.Km21)
            //{
            //    StreamReader r = new StreamReader("Models/Json/workouts5kmPlus.json");
            //    string jsonString = r.ReadToEnd();
            //    JArray JsonCondition = JArray.Parse(jsonString);
            //    List<Dictionary<string, object>> condition = JsonCondition.ToObject<List<Dictionary<string, object>>>();
            //    for (int i = 0; i < condition.Count; i++)
            //    {
            //        var con = condition[i];

            //        if ((string)con["type"] == "Endurance")
            //        {
            //            int tijd = Convert.ToInt32(con["tijd"]);
            //            int tijden = (timeGoal / 5) * (Convert.ToInt32(condition[i]["afstand"]) + tijd);
            //            schemas.Workouts.Add(new EnduranceWorkout() { Title = (string)con["title"], Distance = Convert.ToInt32((con["afstand"])), Time = tijden });

            //        }
            //        else
            //        {
            //            schemas.Workouts.Add(new PowerWorkout() { Title = (string)con["title"] });
            //        }



            //    }
            //    return schemas;

            //}
            //if (score == ConditionScores.Low && distanceGoal == Goals.Km21)
            //{
            //    StreamReader r = new StreamReader("Models/Json/workouts5kmPlus.json");
            //    string jsonString = r.ReadToEnd();
            //    JArray JsonCondition = JArray.Parse(jsonString);
            //    List<Dictionary<string, object>> condition = JsonCondition.ToObject<List<Dictionary<string, object>>>();
            //    for (int i = 0; i < condition.Count; i++)
            //    {
            //        var con = condition[i];

            //        if ((string)con["type"] == "Endurance")
            //        {
            //            int tijd = Convert.ToInt32(con["tijd"]);
            //            int tijden = (timeGoal / 5) * (Convert.ToInt32(condition[i]["afstand"]) + tijd);
            //            schemas.Workouts.Add(new EnduranceWorkout() { Title = (string)con["title"], Distance = Convert.ToInt32((con["afstand"])), Time = tijden });

            //        }
            //        else
            //        {
            //            schemas.Workouts.Add(new PowerWorkout() { Title = (string)con["title"] });
            //        }



            //    }
            //    return schemas;

            //}
            //if (score == ConditionScores.Low && distanceGoal == Goals.Km21)
            //{
            //    StreamReader r = new StreamReader("Models/Json/workouts5kmPlus.json");
            //    string jsonString = r.ReadToEnd();
            //    JArray JsonCondition = JArray.Parse(jsonString);
            //    List<Dictionary<string, object>> condition = JsonCondition.ToObject<List<Dictionary<string, object>>>();
            //    for (int i = 0; i < condition.Count; i++)
            //    {
            //        var con = condition[i];

            //        if ((string)con["type"] == "Endurance")
            //        {
            //            int tijd = Convert.ToInt32(con["tijd"]);
            //            int tijden = (timeGoal / 5) * (Convert.ToInt32(condition[i]["afstand"]) + tijd);
            //            schemas.Workouts.Add(new EnduranceWorkout() { Title = (string)con["title"], Distance = Convert.ToInt32((con["afstand"])), Time = tijden });

            //        }
            //        else
            //        {
            //            schemas.Workouts.Add(new PowerWorkout() { Title = (string)con["title"] });
            //        }



            //    }
            //    return schemas;

            //}
            //Scheme schema2 = new Scheme();
            //return schema2;

        }

    }

}
