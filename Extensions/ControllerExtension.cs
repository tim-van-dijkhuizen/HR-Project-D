using FitbyteServer.Errors;
using FitbyteServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FitbyteServer.Extensions {

    public static class ControllerExtension {

        public static string GetUsername(this ControllerBase controller) {
            HttpRequest request = controller.Request;
            IHeaderDictionary headers = request.Headers;
            StringValues username;

            if (!headers.TryGetValue("Username", out username)) {
               throw new HttpException(400, "Username header is required");
            }

            return username;
        }

        public async static Task<T> GetParam<T>(this ControllerBase controller, string key) {
            HttpRequest request = controller.Request;

            // Read body and convert to JSON
            using (StreamReader reader = new StreamReader(request.Body, Encoding.UTF8)) {  
                string body = await reader.ReadToEndAsync();
                JObject json = JObject.Parse(body);
                
                // Get JSON property
                JToken property;
                
                if(!json.TryGetValue(key, out property)) {
                    return default;
                }

                try {
                    return property.Value<T>();
                } catch(System.FormatException) {
                    throw new HttpException(400, $"Param {key} value invalid");
                }
            }

            throw new HttpException(400, "Failed to parse body");
        }

        public async static Task<T> GetRequiredParam<T>(this ControllerBase controller, string key) {
            HttpRequest request = controller.Request;

            // Read body and convert to JSON
            using (StreamReader reader = new StreamReader(request.Body, Encoding.UTF8)) {  
                string body = await reader.ReadToEndAsync();
                JObject json = JObject.Parse(body);
                
                // Read property
                JToken property;

                if(!json.TryGetValue(key, out property)) {
                    throw new HttpException(400, $"Param {key} is required");
                }

                if(property.IsNullOrEmpty()) {
                    throw new HttpException(400, $"Param {key} is required");
                }

                try {
                    return property.Value<T>();
                } catch(System.FormatException) {
                    throw new HttpException(400, $"Param {key} value invalid");
                }
            }

            throw new HttpException(400, "Failed to parse body");
        }

    }

}
