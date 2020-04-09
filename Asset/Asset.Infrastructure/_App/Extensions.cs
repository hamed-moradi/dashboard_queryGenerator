using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using System.Reflection;
using static Asset.Infrastructure.Common.GeneralEnums;
using System.ComponentModel.DataAnnotations;

namespace Asset.Infrastructure._App
{
    public static class Extensions
    {
        public static string Truncate(this string value, int maxChars = 100)
        {
            return value.Length <= maxChars ? value : value.Substring(0, maxChars) + "...";
        }

        public static string CharacterNormalizer(this string value)
        {
            value = value.Trim();
            //value = value.Replace(",", "_");
            //value = value.Replace("'", "_");
            //value = value.Replace("\"", "_");
            //value = value.Replace("'", "''");
            //value = value.Replace(">", "_>");
            //value = value.Replace("<", "<_");
            //value = value.Replace("/*", " ");
            //value = value.Replace("*/", " ");

            value = value.Replace("ي", "ی");
            value = value.Replace("ك", "ک");
            value = value.Replace("ك", "ڪ");

            //Eastern Arabic (٠	 ١	٢	٣	٤	٥	٦	٧	٨	٩)
            value = value.Replace("١", "1");
            value = value.Replace("٢", "2");
            value = value.Replace("٣", "3");
            value = value.Replace("٤", "4");
            value = value.Replace("٥", "5");
            value = value.Replace("٦", "6");
            value = value.Replace("٧", "7");
            value = value.Replace("٨", "8");
            value = value.Replace("٩", "9");
            value = value.Replace("٠", "0");

            //Perso-Arabic variant (۰	۱	۲	۳	۴	۵	۶	۷	۸	۹)
            value = value.Replace("۱", "1");
            value = value.Replace("۲", "2");
            value = value.Replace("۳", "3");
            value = value.Replace("۴", "4");
            value = value.Replace("۵", "5");
            value = value.Replace("۶", "6");
            value = value.Replace("۷", "7");
            value = value.Replace("۸", "8");
            value = value.Replace("۹", "9");
            value = value.Replace("۰", "0");

            return value;
        }

        public static string ToDescriptionString(this Enum val)
        {
            var attributes = (DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }

        public static string GetDisplayName(this Enum enumValue)
        {
            try
            {
                return enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>()
                .GetName();
            }
            catch (Exception)
            {
                return "غیر مجاز";
            }
        }

        public static IMappingExpression<TSource, TDestination> IgnoreAllVirtual<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression)
        {
            var desType = typeof(TDestination);
            foreach (var property in desType.GetProperties().Where(p => p.Name.ToLower() != "id" && p.GetGetMethod().IsVirtual))
            {
                expression.ForMember(property.Name, opt => opt.Ignore());
            }
            return expression;
        }

        public static IQueryable<T> OrderByField<T>(this IQueryable<T> q, string sortField, bool ascending)
        {
            var param = Expression.Parameter(typeof(T), "p");
            var prop = Expression.Property(param, sortField);
            var exp = Expression.Lambda(prop, param);
            var method = ascending ? "OrderBy" : "OrderByDescending";
            var types = new[] { q.ElementType, exp.Body.Type };
            var mce = Expression.Call(typeof(Queryable), method, types, q.Expression, exp);
            return q.Provider.CreateQuery<T>(mce);
        }
        public static SMSGeteway MapToSMSGateWay(string smsGateWay)
        {
            switch (smsGateWay.ToLower())
            {
                case "vas":
                    return SMSGeteway.vas;
                case "sms.ir":
                    return SMSGeteway.sms_ir;
                case "candoo":
                    return SMSGeteway.candoo;
            }
            return SMSGeteway.Null;
        }
    }
}
