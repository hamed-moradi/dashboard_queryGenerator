using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using Presentation.Panel.Helpers;

namespace Presentation.Panel.FilterAttributes
{
    public class AuthenticationFilter : FilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext context)
        {
            var controller = context.RouteData.GetRequiredString("Controller").ToLower();
            var action = context.RouteData.GetRequiredString("Action").ToLower();
            action = action == string.Empty || action == @"/" ? "/index" : action;
            var method = context.HttpContext.Request.HttpMethod.ToLower();
            if (context.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), false) || context.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), false))
                return;
            if (AdminHelper.AdminId > 0)
            {
                CookieHelper.Set($"/{controller}/{action}");
                if ((controller == "account" && (action == "changepassword" || action == "signout")) || (controller == "home" && action == "index"))
                    return;

                //if (AdminHelper.CheckPermission(AdminHelper.Modules().ToList(), controller, action, method))
                    return;


                //context.Result = new HttpUnauthorizedResult();
                context.Result = new RedirectResult(@"/Home/Error504");
            }
            else
            {
                context.Result = new RedirectResult(@"/Account/SignIn?returnUrl=" + context.HttpContext.Request.RawUrl);
            }
        }
        public void OnAuthenticationChallenge(AuthenticationChallengeContext context)
        {
            //if ((context.Result == null || context.Result is HttpUnauthorizedResult))
            //{
            //    if (UserHelper.UserId == 0)
            //        context.Result = new RedirectToRouteResult("Default", new System.Web.Routing.RouteValueDictionary
            //        {
            //            {"controller", "Account"},
            //            {"action", ""},
            //            {"returnUrl", context.HttpContext.Request.RawUrl}
            //        });
            //    else
            //    {
            //        context.Result = new RedirectToRouteResult("Default", new System.Web.Routing.RouteValueDictionary
            //        {
            //            {"controller", "Home"},
            //            {"action", ""}
            //        });
            //    }
            //}
        }
    }
}