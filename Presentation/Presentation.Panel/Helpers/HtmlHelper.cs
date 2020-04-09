using Presentation.Panel.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;

namespace Presentation.Panel.Helpers
{
    public class HtmlHelpers
    {
        private static HtmlHelpers _instance;
        protected HtmlHelpers()
        {
        }
        public static HtmlHelpers Instance => _instance ?? (_instance = new HtmlHelpers());

        public string YesNo(bool? yesNo)
        {
            return yesNo == true ? "هست" : "نیست";
        }

        public Func<object, string> GetDisplayName = o =>
        {
            var result = null as string;
            var display = o.GetType().GetMember(o.ToString()).First().GetCustomAttributes(false).OfType<DisplayAttribute>().LastOrDefault();
            if (display != null)
                result = display.GetName();
            return result ?? o.ToString();
        };

        public Func<object, string> MakeSearchForm = o =>
        {
            var allProperties = o.GetType().GetProperties().Where(prop => prop.IsDefined(typeof(SearchAttribute), false))
                                .OrderBy(prop => prop.GetCustomAttributes(typeof(SearchAttribute), false).Cast<SearchAttribute>().SingleOrDefault().Type);
            var properties = allProperties.GroupBy(x => x.Name).Select(x => x.First());
            var builder = new StringBuilder();
            builder.Append("<div class='after-before' >");
            foreach (var myProperty in properties)
            {
                var x = myProperty.GetIndexParameters().Length;
                try
                {
                    var searchAttribute = myProperty.GetCustomAttributes(typeof(SearchAttribute), false).Cast<SearchAttribute>().SingleOrDefault();
                    if (searchAttribute == null)
                    {
                        continue;
                    }
                    var Title = myProperty.GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().SingleOrDefault();
                    if (Title == null)
                    {
                        continue;
                    }
                    var Key = myProperty.Name;
                    string typeName = searchAttribute.Type.ToString().ToLower();
                    object value = myProperty.GetValue(o);
                    switch (typeName)
                    {
                        case "enum":
                            var t = myProperty.PropertyType;
                            if (t.IsGenericType)
                            {
                                t = myProperty.PropertyType.GenericTypeArguments[0];
                            }
                            var values = Enum.GetValues(t).Cast<object>().Select(v => new SelectListItem
                            {
                                Selected = v.Equals(value),
                                Text = Instance.GetDisplayName(v),
                                Value = v.ToString()
                            });
                            builder
                                .Append("<div class='searchRow' data-type='")
                                .Append(typeName)
                                .Append("' data-id='")
                                .Append(Key)
                                .Append($"' ><span style='padding-right: 7px;'>{Title.Name}</span><input type='checkbox' ")
                                .Append(value != null ? "checked" : string.Empty)
                                .Append(" /><select id='")
                                .Append(Key)
                                .Append("' name='")
                                .Append(Key)
                                .Append("' ")
                                .Append(value == null ? " disabled" : string.Empty)
                                .Append(">");
                            foreach (var v in values)
                            {
                                builder
                                    .Append("<option value='")
                                    .Append(v.Value)
                                    .Append("' ")
                                    .Append(v.Selected ? "selected" : string.Empty)
                                    .Append(" >")
                                    .Append(v.Text)
                                    .Append("</option>");
                            }
                            builder.Append("</select></div>");
                            break;
                        case "boolean":
                            builder
                                .Append($"<div class='searchRow' data-type='{typeName}' data-id='{Key}' ><span style='padding-right: 7px;'>{Title.Name}</span><input type='checkbox' ")
                                .Append(value != null ? "checked" : string.Empty)
                                .Append($" /><select id='{Key}' name='{Key}'")
                                .Append(value == null ? " disabled" : string.Empty)
                                .Append("><option value='false' ")
                                .Append((bool?)value == false ? " selected" : string.Empty)
                                .Append(">نباشد</option><option value='true'")
                                .Append((bool?)value == true ? " selected" : string.Empty)
                                .Append(">باشد</option></select></div>");
                            break;
                        case "bool":
                            builder
                                .Append("<div class='searchRow searchCheckbox' data-type='")
                                .Append(typeName)
                                .Append("' data-id='")
                                .Append(Key)
                                .Append($"' ><span style='padding-right: 7px;'>{Title.Name}</span><input type='checkbox' ")
                                .Append(value != null ? "checked" : string.Empty)
                                .Append(" /><div class='inner")
                                .Append(value == null ? " disabled" : string.Empty)
                                .Append("' ><input id='")
                                .Append(Key)
                                .Append("' name='")
                                .Append(Key)
                                .Append("' type='checkbox' ")
                                .Append(value == null ? " disabled" : string.Empty)
                                .Append((bool?)value == true ? " checked" : string.Empty)
                                .Append(" value='true' /><input name='")
                                .Append(Key)
                                .Append("' type='hidden' value='false' /><label>")
                                .Append(Title.Name)
                                .Append("</label></div></div>");
                            break;
                        case "number":
                        case "string":
                        case "datetime":
                        case "common":
                            builder
                                .Append("<div class='searchRow' data-type='")
                                .Append(typeName)
                                .Append("' data-id='")
                                .Append(Key)
                                .Append($"' ><span style='padding-right: 7px;'>{Title.Name}</span><input type='checkbox' ")
                                .Append(typeName == "string" ? "checked class='hide'" : string.Empty)
                                .Append((typeName != "string" && value != null) ? "checked" : string.Empty)
                                .Append(" /><input type='")
                                .Append(typeName == "number" ? typeName : "text")
                                .Append("' id='")
                                .Append(Key)
                                .Append("' name='")
                                .Append(Key)
                                .Append("' value='" + value + "' placeholder='")
                                .Append(Title.Name)
                                .Append("' ")
                                .Append((typeName != "string" && value == null) ? " disabled" : string.Empty)
                                .Append(" /></div>");
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    var temp = ex;
                }
            }
            //builder.Append("</div><div class='after-before text-center'><input type=\"submit\" onclick=\"DoAdvancedSearch()\" class=\"btn btn-primary marginR10\" value=\"پیدا کن\" /></div>");
            builder.Append("</div>");
            return builder.ToString();
        };
    }
}