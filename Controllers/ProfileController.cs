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
                profile = new Profile() { Username = ProfileHelper.ParseUsername(username) };
            }

            // Clear schema if goal has changed
            if(profile.Goal != newProfile.Goal) {
                profile.Schema = null;
            }

            // Update profile
            profile.Gender = newProfile.Gender;
            profile.DateOfBirth = newProfile.DateOfBirth;
            profile.Goal = newProfile.Goal;
            profile.Availability = newProfile.Availability;

            // Save profile
            _profileService.SaveProfile(profile);

            return Ok();
        }

        [HttpPost("save-coopertest")]
        public async Task<IActionResult> SaveCoopertest() {
            int distance = await this.GetRequiredParam<int>("distance");

            // Get profile
            string username = this.GetUsername();
            Profile profile = _profileService.GetProfile(username);

            if(profile == null) {
                return NotFound("Profile does not exist");
            }

            // Get condition score and generate schema
            ConditionScores score = _profileService.GetConditionScore(profile.Gender, profile.DateOfBirth, distance);
            Schema schema = _profileService.GenerateSchema(profile.Goal, score);

            // Update and save profile
            profile.Schema = schema;
            _profileService.SaveProfile(profile);

            return Ok(new { score });
        }
  
    }

}
