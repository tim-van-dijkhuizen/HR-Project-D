using FitbyteServer.Base;
using FitbyteServer.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace FitbyteServer.Controllers {

    using Extensions;
    using FitbyteServer.Models;
    using System.Threading.Tasks;

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

            return Ok(new {
                username = "test",
                gender = "M",
                dateOfBirth = new DateTime(),
                goal = Goals.Km5,
                availability = new int[] { 1, 3, 7 },
                conditionScore = ConditionScores.Medium
            });
        }

        [HttpPost("save-profile")]
        public IActionResult SaveProfile([FromBody] Profile profile) {
            string username = this.GetUsername();

            return Ok(new {
                success = true
            });
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
