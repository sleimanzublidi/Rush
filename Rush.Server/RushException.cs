namespace Rush
{
    using System;
    using System.Net;

    public class RushException : Exception
    {
        public RushException()
            : this(HttpStatusCode.OK)
        {}

        public RushException(HttpStatusCode code)
            : this(code, String.Empty)
        {}

        public RushException(HttpStatusCode code, string message)
            : base(message)
        {
            this.StatusCode = code;
        }

        public HttpStatusCode StatusCode { get; private set; }
    }
}
