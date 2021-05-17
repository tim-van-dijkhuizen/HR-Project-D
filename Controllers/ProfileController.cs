using FitbyteServer.Extensions;
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
        public IActionResult SaveProfile([FromBody] Profile profile) {
            string username = this.GetUsername();

            // Get profile
            Profile existing = _profileService.GetProfile(username);

            if(existing != null) {
                existing.Gender = profile.Gender;
                existing.DateOfBirth = profile.DateOfBirth;
                existing.Goal = profile.Goal;
                existing.Availability = profile.Availability;
            } else {
                existing = profile;
            }

            // Save profile
            _profileService.SaveProfile(existing);

            return Ok();
        }

        [HttpPost("save-coopertest")]
        public async Task<IActionResult> SaveCoopertest() {
            string username = this.GetUsername();
            int distance = await this.GetRequiredParam<int>("distance");

            return Ok(new {
                success = true
            });
        }
  
    }

}
