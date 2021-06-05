using System;

namespace FitbyteServer.Errors {

    public class ProfileNotFoundException : Exception {

        public ProfileNotFoundException(string message) : base(message) { }

        public ProfileNotFoundException() : base(null) { }
    
    }

}
