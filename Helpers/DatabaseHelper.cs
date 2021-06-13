using FitbyteServer.Database;
using FitbyteServer.Models;
using FitbyteServer.Records;
using MongoDB.Driver;
using System.Security.Authentication;

namespace FitbyteServer.Helpers {

    public class DatabaseHelper {

        public static MongoClient BuildClient(IDatabaseSettings config) {
            MongoUrl url = new(config.ConnectionString);
            MongoClientSettings settings = MongoClientSettings.FromUrl(url);

            if(config.EnableSsl) {
                settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            }

            return new MongoClient(settings);
        }

        public static ProfileRecord PrepareProfile(string username, Profile profile) {
            return new ProfileRecord() {
                Id = profile.Id,
                Username = username,
                Gender = profile.Gender,
                DateOfBirth = profile.DateOfBirth,
                DistanceGoal = profile.DistanceGoal,
                TimeGoal = profile.TimeGoal,
                Availability = profile.Availability,
                FitbitToken = profile.FitbitToken,
                Scheme = profile.Scheme
            };
        }

        public static Profile ParseProfile(ProfileRecord record) {
            return new Profile() {
                Id = record.Id,
                Gender = record.Gender,
                DateOfBirth = record.DateOfBirth,
                DistanceGoal = record.DistanceGoal,
                TimeGoal = record.TimeGoal,
                Availability = record.Availability,
                FitbitToken = record.FitbitToken,
                Scheme = record.Scheme
            };
        }

    }

}
