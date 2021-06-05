using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitbyteServer.Models;
using FitbyteServer.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FitbyteServer.Controllers
{
    using Extensions;
    using MongoDB.Bson;

    [ApiController]
    [Route("[controller]")]
    public class WorkoutController : ControllerBase
    {

        private readonly WorkoutService _WorkoutService;

        public WorkoutController(WorkoutService workoutService)
        {
          _WorkoutService = workoutService;
        }

        [HttpGet("week-overview")]
        public async Task<IActionResult> WeekOverview()
        {
            // Get profile
            string username = this.GetUsername();
            List<Workout> listofworkout = _WorkoutService.Weekoverview(username);
            double progress = _WorkoutService.GetWeekProcentage(username);
            BsonArray days = _WorkoutService.GetWeekDays(username);
            Console.WriteLine(progress.ToString());
            return Ok(new
            {
                progressPercentage = progress,
                daysofweek = days,
                workouts = listofworkout
                //workouts = new List<Workout> { new Workout { Id = "001", Title = "5km", Endurance = true, Time = 200, Distance = 5000, Speed = 7.0f, DateAccomplished = DateTime.Today } }

            }); ; ;


        }

        [HttpGet("get-progress")]
        public IActionResult GetProgress()
        {
            return Ok(new
            {
                totalPercentage = 100.0f,
                totalDistance = 5000,
                totalWorkouts = 620,
                averageSpeed = 8.0f,
                maxSpeed = 10.0f,
                averageDistance = 520,
                maxDistance = 750

            });
        }

        [HttpPost("complete-workout/{workoutId:int}")]
        public IActionResult CompleteWorkout([FromBody]WorkoutModelView workoutmodelview,int workoutId) 
        {
            return Ok(new { succes = true, Id = workoutId, Time = workoutmodelview.Time });
 
        }

}
}
