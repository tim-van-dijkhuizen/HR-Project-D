using FitbyteServer.Base;
using FitbyteServer.Extensions;
using FitbyteServer.Helpers;
using FitbyteServer.Models;
using FitbyteServer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FitbyteServer.Controllers {

    [ApiController]
    [Route("[controller]")]
    public class ProfileController : ControllerBase {

        private readonly ProfileService _profileService;

        public ProfileController(ProfileService profileService) {
            _profileService = profileService;
        }

        [HttpGet("get-profile")]
        public IActionResult GetProfile() {
            string username = this.GetUsername();

            // Get profile
            Profile profile = _profileService.GetProfile(username);

            if(profile == null) {
                return NotFound("Profile does not exist");
            }

            return Ok(profile);
        }

        [HttpPost("save-profile")]
        public IActionResult SaveProfile([FromBody] Profile newProfile) {
            string username = this.GetUsername();

            // Get profile
            Profile profile = _profileService.GetProfile(username);

            if(profile == null) {
                profile = new Profile();
            }

            // Clear schema if goal has changed
            if(profile.DistanceGoal != newProfile.DistanceGoal || profile.TimeGoal != newProfile.TimeGoal) {
                profile.Scheme = null;
            }

            // Update profile
            profile.Gender = newProfile.Gender;
            profile.DateOfBirth = newProfile.DateOfBirth;
            profile.DistanceGoal = newProfile.DistanceGoal;
            profile.TimeGoal = newProfile.TimeGoal;
            profile.Availability = newProfile.Availability;

            // Save profile
            _profileService.SaveProfile(username, profile);

            return Ok();
        }

        [HttpPost("save-coopertest")]
        public async Task<IActionResult> SaveCoopertest() {
            int distance = await this.GetRequiredParam<int>("distance");
            string username = this.GetUsername();

            // Get profile
            Profile profile = _profileService.GetProfile(username);

            if(profile == null) {
                return NotFound("Profile does not exist");
            }

            // Get condition score and generate schema
            ConditionScores score = _profileService.GetConditionScore(profile.Gender, profile.DateOfBirth, distance);
            Scheme schema = _profileService.GenerateScheme(profile.DistanceGoal, profile.TimeGoal, score);

            // Update and save profile
            profile.Scheme = schema;
            _profileService.SaveProfile(username, profile);

            return Ok(new { score });
        }

        [HttpGet("get-fitbit-token")]
        public IActionResult GetFitbitToken() {
            string username = this.GetUsername();

            // Get profile
            Profile profile = _profileService.GetProfile(username);

            if(profile == null) {
                return NotFound("Profile does not exist");
            }

            return Ok(new { Token = profile.FitbitToken });
        }

        [HttpPost("save-fitbit-token")]
        public async Task<IActionResult> SaveFitbitToken() {
            string username = this.GetUsername();
            string accessToken = await this.GetRequiredParam<string>("AccessToken");
            string refreshToken = await this.GetRequiredParam<string>("RefreshToken");

            // Get profile
            Profile profile = _profileService.GetProfile(username);

            if(profile == null) {
                return NotFound("Profile does not exist");
            }

            // Set token and save
            profile.FitbitToken = new FitbitToken() {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            _profileService.SaveProfile(username, profile);

            return Ok();
        }
  
    }

}
