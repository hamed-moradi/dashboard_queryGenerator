using System.Web.Mvc;
using Asset.Infrastructure._App;
using Presentation.Panel.Models;
using System.Linq;
using System;
using System.Collections.Generic;
using Asset.Infrastructure.Common;

namespace Presentation.Panel.FilterAttributes
{
    public class SecurityFilter : ActionFilterAttribute, IActionFilter
    {
        #region Constructor
        private readonly ILog4Net _logger;
        private readonly string[] UnsafeKeywords = { "javascript", "vbscript", "shutdown", "exec", "having", "union", "select", "insert", "update", "delete", "drop", "truncate", "script" };

        public SecurityFilter()
        {
            _logger = ServiceLocatorAdapter.Current.GetInstance<ILog4Net>();
        }
        #endregion

        #region Private
        private void KeywordChecker(ActionExecutingContext filterContext, string text)
        {
            if (UnsafeKeywords.Contains(text.ToLower()))
            {
                _logger.Info(new
                {
                    UserId = filterContext.HttpContext.User.Identity.Name,
                    IP = filterContext.HttpContext.Request.UserHostAddress,
                    Controller = filterContext.Controller,
                    Keyword = text,
                    Message = "Restricted keyword detection"
                });
                throw new Exception("درخواست شما شامل کلمات خطرناک می باشد. آی پی شما ثبت شد!", new Exception { Source = GeneralMessages.ExceptionSource });
            }
        }
        #endregion

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            foreach (var param in filterContext.ActionParameters)
            {
                if (param.Value is string && param.Value != null)
                {
                    //filterContext.ActionParameters[param.Key] = new KeyValuePair<string, object>(param.Key, param.Value.ToString().CharacterNormalizer());
                    KeywordChecker(filterContext, param.Value.ToString());
                }
                if (param.Value is BaseViewModel)
                {
                    var properties = param.Value.GetType().GetProperties();
                    foreach (var item in properties)
                    {
                        if (item.PropertyType == typeof(string) && item != null)
                        {
                            //filterContext.ActionParameters[param.Key] = new KeyValuePair<string, object>(param.Key, param.Value.ToString().CharacterNormalizer());
                            KeywordChecker(filterContext, item.ToString());
                        }
                    }
                }
            }
        }
    }
}