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
    [ApiController]
    [Route("[controller]")]
    public class WorkoutController : ControllerBase
    {

        //private readonly WorkoutService _WorkoutService;

        //public WorkoutController(WorkoutService workoutService)
        //{
            //_WorkoutService = workoutService;
        //}

        [HttpGet("week-overview")]
        public IActionResult WeekOverview()
        {
            return Ok(new
            {
                progressPercentage = 37.0f,
                days = new Dictionary<int, string>{

                { 1,"avaliable"},
                { 3,"avaliable"},
                {7,"avaliable"}

                },
                workouts = new List<Workout> { new Workout {Id =  "001",Title =  "5km", Endurance = true, Time = 200, Distance = 5000, Speed = 7.0f, DateAccomplished = new DateTime()} }

            });

            
        }
        [HttpGet("get-progress")]
        public IActionResult GetProgress()
        {
            return Ok(new {
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
