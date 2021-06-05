using System;

namespace FitbyteServer.Errors {

    public class WorkoutNotFoundException : Exception {

        public WorkoutNotFoundException(string message) : base(message) { }

        public WorkoutNotFoundException() : base(null) { }
    
    }

}
