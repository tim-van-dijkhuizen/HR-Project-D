using System;
using System.Linq;
using System.Collections.Generic;
using FitbyteServer.Extensions;
using FitbyteServer.Models;
using MongoDB.Driver;
using FitbyteServer.Errors;
using FitbyteServer.Base;
using Newtonsoft.Json.Linq;

namespace FitbyteServer.Services {

    public class WorkoutService {
        
        private readonly ProfileService _profileService;

        public WorkoutService(ProfileService profileService) {
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
            Dictionary<int, string> days = new();

            for(int i = 1; i <= 7; i++) {
                string status = "unavailable";

                // Check if available
                if(profile.Availability.Contains(i)) {
                    status = "available";
                }

                // Check if completed
                bool filter(Workout w) {
                    DateTime? completed = w.DateCompleted;

                    if(completed != null) {
                        int dayOfWeek = (int) completed.Value.DayOfWeek;
                        return dayOfWeek == i;
                    }

                    return false;
                }

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

        public void CompleteWorkout(string username, string workoutId, JObject json) {
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

            // Find workout
            Workout workout = scheme.Workouts.Where(w => w.Id == workoutId).FirstOrDefault();

            if(workout == null) {
                throw new WorkoutNotFoundException();
            }

            try {
                workout.SetResult(json);
            } catch(Exception e) {
                throw new InvalidResultException(e.Message);
            }
           
            workout.DateCompleted = DateTime.Now;

            // Re-generate progress
            scheme.Progress = GenerateProgress(scheme);

            // Save profile
            _profileService.SaveProfile(username, profile);
        }

        private Progress GenerateProgress(Scheme scheme) {
            List<Workout> workouts = scheme.Workouts;

            List<Workout> completed = workouts
                .Where(w => w.DateCompleted != null)
                .ToList();

            List<EnduranceWorkout> enduranceWorkouts = completed
                .OfType<EnduranceWorkout>()
                .ToList();

            int completedCount = completed.Count;
            int totalCount = workouts.Count;

            // Build progress
            return new Progress {
                TotalPercentage = (float) completedCount / totalCount * 100f,
                
                TotalDistance = enduranceWorkouts.Sum(w => w.Result.Distance),
                TotalWorkouts = completedCount,

                AverageSpeed = enduranceWorkouts.Any() ? enduranceWorkouts.Average(w => w.Result.Speed) : 0f,
                MaxSpeed = enduranceWorkouts.Any() ? enduranceWorkouts.Max(w => w.Result.Speed) : 0f,

                AverageDistance = enduranceWorkouts.Any() ? (int) enduranceWorkouts.Average(w => w.Result.Distance) : 0,
                MaxDistance = enduranceWorkouts.Any() ? enduranceWorkouts.Max(w => w.Result.Distance) : 0
            };
        }

    }

}



