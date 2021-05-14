using FitbyteServer.Base;
using FitbyteServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitbyteServer.Controllers {

    [ApiController]
    [Route("[controller]")]
    public class ProfileController : ControllerBase {

        private readonly ProfileService _profileService;

        public ProfileController(ProfileService profileService) {
            _profileService = profileService;
        }

        [HttpPost("create")]
        public string Create([FromBody] Profile profile) {
            bool success = _profileService.CreateProfile(profile);
            return "Success: " + success;
            
        }
  


}

}
