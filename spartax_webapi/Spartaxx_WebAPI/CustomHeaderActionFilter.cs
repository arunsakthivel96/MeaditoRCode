using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace Spartaxx_WebAPI
{
    public class CustomHeaderActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
           if (actionExecutedContext.Response != null)
            {
                actionExecutedContext.Response.Headers.Add("ServerName", "Localhost");
            }

            base.OnActionExecuted(actionExecutedContext);
        }
    }
}