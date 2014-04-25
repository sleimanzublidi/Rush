namespace Rush
{
    using System;
    using System.Collections.Generic;
    using System.Web;

    public enum HttpVerb { Unsupported, POST, GET, PUT, DELETE }
    
    public class RushContext
    {
        public RushContext(HttpContext context)
        {
            this.HttpContext = context;

            HttpVerb verb = HttpVerb.Unsupported;
            Enum.TryParse(context.Request.RequestType, true, out verb);
            this.Verb = verb;

            this.RawUrl = context.Request.AppRelativeCurrentExecutionFilePath.Replace("~/", "");
            
            string[] segments = RawUrl.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            if (segments.Length == 3)
            {
                this.Controller = segments[1];
                this.Resource = segments[2];
            }
            if (segments.Length == 4)
            {
                this.Controller = segments[1] + "RushController";
                this.Resource = segments[2];
                this.Id = segments[3];
            }

            this.Headers = new Dictionary<string, string>();
            foreach (var key in context.Request.Headers.AllKeys)
            {
                this.Headers[key] = context.Request.Headers[key];
            }

            this.QueryString = new Dictionary<string, string>();
            foreach (var key in context.Request.QueryString.AllKeys)
            {
                this.QueryString[key] = context.Request.QueryString[key];
            }
        }

        public HttpContext HttpContext { get; private set; }

        public HttpVerb Verb { get; private set; }
        public string RawUrl { get; private set; }

        public string[] Prefix { get; private set; }
        public string Controller { get; private set; }
        public string Resource { get; private set; }
        public string Id { get; private set; }

        public Dictionary<string, string> Headers { get; private set; }
        public Dictionary<string, string> QueryString { get; private set; }        
    }
}
