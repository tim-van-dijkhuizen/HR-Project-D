using FitbyteServer.Base;
using FitbyteServer.Validators;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FitbyteServer.Models {

    public class Profile {

        public string Id { get; init; }
        
        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        public Genders Gender { get; set; }

        [Required]
        [DateOfBirth]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        public Goals DistanceGoal { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "TimeGoal must be one or higher")]
        public int TimeGoal { get; set; }

        [Required]
        [Availability]
        public List<int> Availability { get; set; }

        public FitbitToken FitbitToken { get; set; }

        public Scheme Scheme { get; set; }

    }

}
