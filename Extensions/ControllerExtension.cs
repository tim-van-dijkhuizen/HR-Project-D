using FitbyteServer.Errors;
using FitbyteServer.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace FitbyteServer.Extensions {

    public static class ControllerExtension {

        public static string GetUsername(this ControllerBase controller) {
            HttpRequest request = controller.Request;
            IHeaderDictionary headers = request.Headers;

            if(!headers.TryGetValue("Username", out StringValues username)) {
                throw new HttpException(400, "Username header is required");
            }

            return username;
        }

        public async static Task<T> GetParam<T>(this ControllerBase controller, string key) {
            HttpRequest request = controller.Request;
            JObject json = await request.GetBodyAsJson();

            try {
                return JsonHelper.GetValue<T>(json, key);
            } catch(InvalidJsonPropertyException) {
                throw new HttpException(400, $"Param {key} is invalid");
            }
        }

        public async static Task<T> GetRequiredParam<T>(this ControllerBase controller, string key) {
            HttpRequest request = controller.Request;
            JObject json = await request.GetBodyAsJson();

            try {
                return JsonHelper.GetValue<T>(json, key);
            } catch(MissingJsonPropertyException) {
                throw new HttpException(400, $"Param {key} is required");
            } catch(InvalidJsonPropertyException) {
                throw new HttpException(400, $"Param {key} is invalid");
            }
        }

    }

}
