using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FitbyteServer.Errors;
using FitbyteServer.Extensions;
using FitbyteServer.Models;
using FitbyteServer.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace FitbyteServer.Controllers {

    [ApiController]
    [Route("[controller]")]
    public class WorkoutController : ControllerBase {

        private readonly WorkoutService _workoutService;

        public WorkoutController(WorkoutService workoutService) {
            _workoutService = workoutService;
        }

        [HttpGet("get-week-overview")]
        public IActionResult WeekOverview() {
            string username = this.GetUsername();

            try {
                return Ok(_workoutService.GetWeekOverview(username));
            } catch(ProfileNotFoundException) {
                return BadRequest("Profile does not exist");
            } catch(SchemeNotFoundException) {
                return BadRequest("Scheme does not exist");
            }
        }

        [HttpPost("complete-workout")]
        public async Task<IActionResult> CompleteWorkout() {
            string username = this.GetUsername();
            string workoutId = await this.GetRequiredParam<string>("workoutId");
            int time = await this.GetRequiredParam<int>("time");

            try {
                _workoutService.CompleteWorkout(username, workoutId, time);
            } catch(WorkoutNotFoundException) {
                return BadRequest("Workout does not exist");
            }

            return Ok();
        }

        [HttpGet("get-progress")]
        public IActionResult GetProgress() {
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

    }
}
