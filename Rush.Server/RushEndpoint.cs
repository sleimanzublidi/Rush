namespace Rush
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Routing;
    using Newtonsoft.Json;

    public class RushEndpoint : IHttpHandler, IRouteHandler
    {
        private RushAuthorizer authorizer = new RushAuthorizer();

        public RushEndpoint()
        {}

        public virtual bool IsReusable { get { return true; } }

        public IHttpHandler GetHttpHandler(RequestContext requestContext) { return this; }

        public virtual void ProcessRequest(HttpContext httpContext)
        {
            try
            {
                var context = new RushContext(httpContext);
                //authorizer.Authorize(context);

                if (String.IsNullOrWhiteSpace(context.Controller) == false)
                {
                    var controllerType = this.GetType().Assembly.GetTypes().FirstOrDefault(t => t.Name.ToLower() == context.Controller.ToLower());
                    if (controllerType != null)
                    {
                        var controller = (IRushController)Activator.CreateInstance(controllerType, new object[] { context });
                        if (controller != null)
                        {
                            switch (context.Verb)
                            {
                                case HttpVerb.POST:
                                    controller.Post();
                                    return;
                                case HttpVerb.GET:
                                    controller.Get();
                                    return;
                                case HttpVerb.PUT:
                                    controller.Put();
                                    return;
                                case HttpVerb.DELETE:
                                    controller.Delete();
                                    return;
                                default:
                                    break;
                            }
                        }
                    }
                }

                httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            catch (RushException ex)
            {
                var json = JsonConvert.SerializeObject(new { Error = ex.Message });
                httpContext.Response.StatusCode = (int)ex.StatusCode;
                httpContext.Response.Write(json);
            }
            catch (Exception ex)
            {
                var json = JsonConvert.SerializeObject(new { Error = ex.Message });
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.Write(json);
            }
        }
    }
}
