using Presentation.Panel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
namespace Presentation.Panel.Helpers
{
    public static class GridViewTreeHtmlHelperExtensions
    {
        public static MvcHtmlString RoleGridViewTrs<TModel, TResult>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TResult>> expression)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            var fieldName = metadata.PropertyName;
            var model = metadata.Model;
            var entities = model as IEnumerable<RoleViewModel>;

            string GetTreeTr(IEnumerable<RoleViewModel> items)
            {
                var trs = string.Empty;
                foreach (var item in items)
                {
                    trs +=
                        $"<tr data-gid='{item.Id}' data-gparentid='{item.Id ?? -1}'>" +
                        $"    <td>{item.Title}</td>" +
                        $"    <td>{item.Description}</td>" +
                        $"    <td>" +
                        $"        {helper.ActionLink("دسترسی های نقش", "Permission", new { id = item.Id })} |" +
                        $"        {helper.ActionLink("ویرایش", "Edit", new { id = item.Id })} |" +
                        $"        {helper.ActionLink("جزئیات", "Details", new { id = item.Id })}" +
                        $"    </td>" +
                        $"</tr>";

                    //trs += GetTreeTr(items);
                }

                return trs;
            }

            var html = GetTreeTr(entities);
            return MvcHtmlString.Create(HttpUtility.HtmlDecode(html));
        }
    }
}