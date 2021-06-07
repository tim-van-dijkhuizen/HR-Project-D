using FitbyteServer.Errors;
using FitbyteServer.Extensions;
using Newtonsoft.Json.Linq;
using System;

namespace FitbyteServer.Helpers {

    public class JsonHelper {

        public static T GetValue<T>(JObject json, string key) {
            if(!json.TryGetValue(key, out JToken property)) {
                return default;
            }

            try {
                return property.Value<T>();
            } catch(FormatException) {
                throw new InvalidJsonPropertyException("Property has an invalid value");
            }
        }

        public static T GetRequiredValue<T>(JObject json, string key) {
            if(!json.TryGetValue(key, out JToken property)) {
                throw new MissingJsonPropertyException("Property does not exist");
            }

            if(property.IsNullOrEmpty()) {
                throw new MissingJsonPropertyException("Property is null or empty");
            }

            try {
                return property.Value<T>();
            } catch(FormatException) {
                throw new InvalidJsonPropertyException("Property has an invalid value");
            }
        }

    }

}
