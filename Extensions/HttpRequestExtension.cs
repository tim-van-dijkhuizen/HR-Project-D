using FitbyteServer.Errors;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FitbyteServer.Extensions {

    public static class HttpRequestExtension {
        
        public async static Task<string> GetBody(this HttpRequest request) {
            HttpRequestRewindExtensions.EnableBuffering(request);

            // Read body
            using (StreamReader reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true)) {
                string body = await reader.ReadToEndAsync();

                request.Body.Position = 0;

                return body;
            }
                
            throw new HttpException(400, "Failed to read body");
        }

        public async static Task<JObject> GetBodyAsJson(this HttpRequest request) {
            string body = await GetBody(request);
            
            try {
                return JObject.Parse(body);
            } catch(JsonReaderException) {
                throw new HttpException(400, "Failed to parse body");
            }
        }

    }

}
