using Spartaxx.Utilities.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Spartaxx_WebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            UnityConfig.RegisterComponents();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //added by saravanans for create logs-tfs:47247
            TransactionLogger.Initialize();
            InvoiceLogger.Initialize();
            //ends
            // Register the custom action filter globally
            GlobalConfiguration.Configuration.Filters.Add(new CustomHeaderActionFilter());

        }
    }
}
