using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Template.Library.Exceptions
{
    [Serializable()]
    public class GeneralException : Exception
    {
        public GeneralException() : base() { }
        public GeneralException(string message) : base(message) { }
        public GeneralException(string message, Exception inner) : base(message, inner) { }
        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client.
        protected GeneralException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
