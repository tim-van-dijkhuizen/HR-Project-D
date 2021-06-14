using MongoDB.Driver;
using FitbyteServer.Models;
using System;
using System.Collections.Generic;
using FitbyteServer.Base;
using FitbyteServer.Helpers;
using FitbyteServer.Records;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

        public Scheme Generate(int timeGoal, ConditionScores score, string workout)
        {
            Scheme schemas = new Scheme();

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

        public Scheme GenerateScheme(Goals distanceGoal, int timeGoal, ConditionScores score) {
            //Scheme schemas = new Scheme();
            //schemas.Workouts = new List<Workout>();
            //schemas.WorkoutsPerWeek = daysAvailable;
            //schemas.ConditionScore = score;
            //string workout = "Models/Json/";
            if ((score == ConditionScores.Low || score == ConditionScores.Lowest || score == ConditionScores.Medium) && distanceGoal == Goals.Km5)
            {
                return Generate(timeGoal, score, "km5Min.json");


            }
      
            if ((score == ConditionScores.Medium || score == ConditionScores.High || score == ConditionScores.Highest) && distanceGoal == Goals.Km5)
            {
                return Generate(timeGoal, score, "workouts5kmPlus.json");
            }
           
            if ((score == ConditionScores.Low || score ==  ConditionScores.Lowest ) && distanceGoal == Goals.Km10)
            {
                Scheme schema2 = Generate(timeGoal, score, "10kmMini.json");
                Scheme schema1 = Generate(timeGoal, score, "foundation.json");
                var newSchema = new Scheme();

                schema1.Workouts.ForEach(item => newSchema.Workouts.Add(item));
                schema2.Workouts.ForEach(item => newSchema.Workouts.Add(item));
                
                newSchema.ConditionScore = score;
                
                return newSchema;
            }
            if(score == ConditionScores.Medium && distanceGoal == Goals.Km10)
            {
                return Generate(timeGoal, score, "10kmMini.json");

            }
            if ((score == ConditionScores.High || score == ConditionScores.Highest) && distanceGoal == Goals.Km10)
            {
                return Generate(timeGoal, score, "10kmPlus.json");
            }
            if (score == ConditionScores.Lowest  && distanceGoal == Goals.Km21)
            {
                Scheme schema2 = Generate(timeGoal, score, "10kmMini.json");
                Scheme schema1 = Generate(timeGoal, score, "foundation.json");
                Scheme schema3 = Generate(timeGoal, score, "21kmMini.json");
                var newSchema = new Scheme();

                schema1.Workouts.ForEach(item => newSchema.Workouts.Add(item));
                schema2.Workouts.ForEach(item => newSchema.Workouts.Add(item));
                schema3.Workouts.ForEach(item => newSchema.Workouts.Add(item));
                
                newSchema.ConditionScore = score;
                
                return newSchema;
            }
            if(score == ConditionScores.Medium && distanceGoal == Goals.Km21)
            {
                Scheme schema10km = Generate(timeGoal, score, "10kmPlus.json");
                Scheme schema = Generate(timeGoal, score, "21kmMini");
                var newSchema = new Scheme();

                schema10km.Workouts.ForEach(item => newSchema.Workouts.Add(item));
                schema.Workouts.ForEach(item => newSchema.Workouts.Add(item));
                
                newSchema.ConditionScore = score;
                
                return newSchema;



            }
            if(score == ConditionScores.High && distanceGoal == Goals.Km21)
            {
                Scheme schema10km = Generate(timeGoal, score, "10kmPlus.json");
                Scheme schema = Generate(timeGoal, score, "21kmMax.json");
                var newSchema = new Scheme();

                schema10km.Workouts.ForEach(item => newSchema.Workouts.Add(item));
                schema.Workouts.ForEach(item => newSchema.Workouts.Add(item));

                newSchema.ConditionScore = score;
                
                return newSchema;

            }
            if(score == ConditionScores.Highest && distanceGoal == Goals.Km21)
            {
                Scheme schema = Generate(timeGoal, score, "21kmMax.json");
                return schema;
            }
            if(score == ConditionScores.Lowest && distanceGoal == Goals.Km42)
            {
                Scheme schema1 = Generate(timeGoal, score, "foundation.json");

                return new Scheme();
            }
            if(score == ConditionScores.Low && distanceGoal == Goals.Km42)
            {
                return new Scheme();
            }
            if (score == ConditionScores.Medium && distanceGoal == Goals.Km42)
            {
                return new Scheme();
            }
            if (score == ConditionScores.High && distanceGoal == Goals.Km42)
            {
                return new Scheme();
            }
            if (score == ConditionScores.Highest && distanceGoal == Goals.Km42)
            {
                return new Scheme();
            }

            return new Scheme();
        }

    }

}
