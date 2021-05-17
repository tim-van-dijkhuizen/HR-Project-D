using MongoDB.Driver;
using FitbyteServer.Extensions;
using FitbyteServer.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace FitbyteServer.Services {

    public class ProfileService {

        private readonly IMongoCollection<Profile> _profiles;

        public ProfileService(IMongoDatabase database) {
            _profiles = database.GetCollection<Profile>("profiles");
        }

        public Profile GetProfile(string username) {
            return _profiles.Find<Profile>(profile => profile.Username == username.ToLower())
                .FirstOrDefault();
        }

        public void SaveProfile(Profile profile) {
            string username = profile.Username.ToLower();
            Profile existing = GetProfile(username);

            // Create if its a new profile
            if(existing == null) {
                _profiles.InsertOne(profile);
                return;
            }

            // Update existing profile
            _profiles.ReplaceOne(p => p.Username == username, profile);
        }

    }

}
