using System;

namespace FitbyteServer.Errors {

    public class MissingJsonPropertyException : Exception {

        public MissingJsonPropertyException(string message) : base(message) { }

        public MissingJsonPropertyException() : base(null) { }
    
    }

}
