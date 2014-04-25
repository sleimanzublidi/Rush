namespace Rush
{
    using System;
    using System.Net;

    public class RushAuthorizer
    {
        public void Authorize(RushContext context)
        {
            string applicationId = null;
            context.Headers.TryGetValue("X-Rush-Application-Id", out applicationId);

            string applicationKey = null;
            context.Headers.TryGetValue("X-Rush-Application-Key", out applicationKey);

            if (String.IsNullOrWhiteSpace(applicationId) || String.IsNullOrWhiteSpace(applicationKey))
            {
                throw new RushException(HttpStatusCode.Unauthorized, "unknown application");
            }

            //TODO: Validate Application Id and Key
        }
    }
}
