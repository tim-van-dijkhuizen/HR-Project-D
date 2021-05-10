using FitbyteServer.Base;
using MongoDB.Driver;
using System.Security.Authentication;

namespace FitbyteServer.Helpers {

    public class DatabaseHelper {

        public static MongoClient BuildClient(IDatabaseSettings config) {
            MongoUrl url = new MongoUrl(config.ConnectionString);
            MongoClientSettings settings = MongoClientSettings.FromUrl(url);

            if(config.EnableSsl) {
                settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            }

            return new MongoClient(settings);
        }

    }

}
