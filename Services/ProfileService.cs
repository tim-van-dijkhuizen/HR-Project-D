﻿using FitbyteServer.Helpers;
using FitbyteServer.Database;
using MongoDB.Driver;
using System;
using FitbyteServer.Models;

namespace FitbyteServer.Services {

    public class ProfileService {

        private readonly IMongoCollection<Profile> _profiles;

        public ProfileService(IDatabaseSettings settings) {
            MongoClient client = DatabaseHelper.BuildClient(settings);
            IMongoDatabase database = client.GetDatabase(settings.DatabaseName);

            _profiles = database.GetCollection<Profile>("profiles");
        }

        public bool CreateProfile(Profile profile) {
            try {
                _profiles.InsertOne(profile);
                return true;
            } catch(Exception) {
                return false;
            }
        }

    }

}
