using System;

namespace FitbyteServer.Errors {

    public class SchemeNotFoundException : Exception {

        public SchemeNotFoundException(string message) : base(message) { }

        public SchemeNotFoundException() : base(null) { }
    
    }

}
