using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ToilluminateModel
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            // add players to PlayerStatusDic
            if (Application["playerHeartBeat"] == null)
            {
                Dictionary<int, string> playerHeartBeatDic = new Dictionary<int, string>();
                Application.Add("playerHeartBeat", playerHeartBeatDic);
            }
        }
        protected void Application_End() {

        }
    }
}
