using System;

namespace FitbyteServer.Errors {

    public class InvalidResultException : Exception {

        public InvalidResultException(string message) : base(message) { }

        public InvalidResultException() : base(null) { }
    
    }

}
