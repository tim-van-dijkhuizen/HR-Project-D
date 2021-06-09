using System;

namespace FitbyteServer.Errors {

    public class InvalidJsonPropertyException : Exception {

        public InvalidJsonPropertyException(string message) : base(message) { }

        public InvalidJsonPropertyException() : base(null) { }
    
    }

}
