using System.Threading.Tasks;
using FitbyteServer.Errors;
using FitbyteServer.Extensions;
using FitbyteServer.Models;
using FitbyteServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitbyteServer.Controllers {

    [ApiController]
    [Route("[controller]")]
    public class WorkoutController : ControllerBase {

        private readonly ProfileService _profileService;
        private readonly WorkoutService _workoutService;

        public WorkoutController(ProfileService profileService, WorkoutService workoutService) {
            _profileService = profileService;
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
            string username = this.GetUsername();
            
            // Make sure the profile exists
            Profile profile = _profileService.GetProfile(username);

            if(profile == null) {
                return BadRequest("Profile does not exist");
            }

            // Make sure the schema exists
            Scheme scheme = profile.Scheme;

            if(scheme == null) {
                return BadRequest("Scheme does not exist");
            }

            return Ok(new {
                distanceGoal = profile.DistanceGoal.ToString(),
                timeGoal = profile.TimeGoal,
                progress = scheme.Progress
            });
        }


    }
}
