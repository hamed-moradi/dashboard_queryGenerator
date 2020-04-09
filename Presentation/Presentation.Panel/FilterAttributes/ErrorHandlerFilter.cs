using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Asset.Infrastructure._App;

using System.Net;
using System.Linq;
using Asset.Infrastructure.Common;

namespace Presentation.Panel.FilterAttributes
{
    public class ErrorHandlerFilter : HandleErrorAttribute
    {
        private readonly ILog4Net _logger;

        public ErrorHandlerFilter()
        {
            _logger = ServiceLocatorAdapter.Current.GetInstance<ILog4Net>();
        }

        public override void OnException(ExceptionContext filterContext)
        {
            _logger.Error(filterContext.Exception);
            string viewName = "~/Views/Home/Error.cshtml";
            var httpContext = HttpContext.Current;
            if (httpContext == null) return;
            if (filterContext.ExceptionHandled || filterContext.HttpContext.IsCustomErrorEnabled) return;
            if (filterContext.Exception is HttpException)
            {
                //A http exception has occourd
            }
            else if (filterContext.Exception is UnauthorizedAccessException)
            {
                httpContext.Response.Redirect(@"/Home/Page504");
            }
            else if(filterContext.Exception is WebException)
            {
                httpContext.Response.Redirect(@"/Home/Page404");
            }
            filterContext.ExceptionHandled = true;
            var actionName = (string)filterContext.RouteData.Values["action"];
            var controllerName = (string)filterContext.RouteData.Values["controller"];
            var requestContext = ((MvcHandler)httpContext.CurrentHandler).RequestContext;
            if (requestContext.HttpContext.Request.IsAjaxRequest())
            {
                httpContext.Response.Clear();
                var factory = ControllerBuilder.Current.GetControllerFactory();
                var controller = factory.CreateController(requestContext, controllerName);
                var controllerContext = new ControllerContext(requestContext, (ControllerBase)controller);
                var jsonResult = new JsonResult
                {
                    Data = new { success = false, serverError = "500" },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
                jsonResult.ExecuteResult(controllerContext);
                httpContext.Response.End();
            }
            else
            {
                var viewResult = new ViewResult
                {
                    MasterName = Master,
                    TempData = filterContext.Controller.TempData,
                    ViewName = viewName
                };
                if (filterContext.Exception.InnerException != null && filterContext.Exception.InnerException.Source.Equals(GeneralMessages.ExceptionSource))
                {
                    viewResult.ViewData = new ViewDataDictionary(filterContext.Exception.Message);
                }
                filterContext.Result = viewResult;
            }
        }
    }
}