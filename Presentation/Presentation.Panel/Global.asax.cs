using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Presentation.Panel.Components;
using Presentation.Panel.Helpers;

namespace Presentation.Panel
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ModuleInjector.Init();
        }

        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            CookieHelper.Set();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            //Server.ClearError();
            //Response.Redirect("/Home/ErrorPage");
        }
    }
}
