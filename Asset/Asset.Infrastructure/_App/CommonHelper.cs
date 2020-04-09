using System;
using System.Reflection;
using System.Text;
using System.Web.ModelBinding;
using System.Collections;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

namespace Asset.Infrastructure._App
{
    public static class CommonHelper
    {
        public static string Md5Password(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) return null;
            var md5 = System.Security.Cryptography.MD5.Create();
            var inputBytes = Encoding.ASCII.GetBytes(password);
            var hash = md5.ComputeHash(inputBytes);
            var sb = new StringBuilder();
            foreach (var t in hash)
            {
                sb.Append(t.ToString("x2"));
            }
            return sb.ToString();
        }

        public static void ViewModelErrorManager(ModelStateDictionary modelState)
        {
            foreach (var item in modelState.Values)
            {
                if (item.Errors.Count <= 0) continue;
                foreach (var error in item.Errors)
                {
                    modelState.AddModelError(string.Empty, error.ErrorMessage);
                }
            }
        }
        public static PropertyInfo GetLowestProperty(Type type, string name)
        {
            while (type != null)
            {
                var property = type.GetProperty(name, BindingFlags.DeclaredOnly |
                                                      BindingFlags.Public |
                                                      BindingFlags.Instance);
                if (property != null)
                {
                    return property;
                }
                type = type.BaseType;
            }
            return null;
        }
        private static bool IsSimple(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                // nullable type, check if the nested type is simple.
                return IsSimple(type.GetGenericArguments()[0]);
            }
            return type.IsPrimitive
              || type.IsEnum
              || type.Equals(typeof(string))
              || type.Equals(typeof(decimal))
              || type.Namespace == "System.Data.SqlTypes";
        }
        public static XElement GetPropertyValue(object o, string name)
        {
            var typeName = o.GetType().Name.ToString().ToLower();

            if (!IsSimple(o.GetType()))
            {
                return o.GetXmlElement(name);
            }
            if (o.ToString() == "Null")
            {
                return null;
            }
            var elem = new XElement(name);
            if (typeName == "boolean")
            {
                elem.Value = ((bool)o ? "1" : "0");
            }
            else
            {
                elem.Value = o.ToString();
            }
            return elem;
        }
        private static XElement GetPropertyElement(PropertyInfo p, ref object obj)
        {
            var root = new XElement(p.Name);
            if (p.PropertyType.IsArray)
            {
                Array a = (Array)p.GetValue(obj);
                for (int i = 0; i < a.Length; i++)
                {
                    var o = a.GetValue(i);
                    if (o == null)
                    {
                        break;
                    }
                    root.Add(GetPropertyValue(o, "b"));
                }
                return root;
            }
            else if (p.PropertyType.Name == "List`1")
            {
                var a = ((IList)p.GetValue(obj, null)).Cast<object>().ToArray();
                for (int i = 0; i < a.Length; i++)
                {
                    var o = a.GetValue(i);
                    if (o == null)
                    {
                        break;
                    }
                    root.Add(GetPropertyValue(o, "a"));
                }
                return root;
            }
            var pObject = p.GetValue(obj, null);
            return GetPropertyValue(pObject, p.Name);
        }
        public static XElement GetXmlElement(this object obj, string name = null)
        {
            //var x = (from p in obj.GetType().GetProperties()
            //         where p.GetValue(obj, null) != null && p.PropertyType.Name != obj.GetType().Name
            //         select p).ToArray();
            var properties = (from p in obj.GetType().GetProperties()
                              where p.GetValue(obj, null) != null && p.PropertyType.Name != obj.GetType().Name
                              select GetPropertyElement(p, ref obj)).ToArray();
            if (string.IsNullOrWhiteSpace(name))
            {
                name = obj.GetType().Name;
            }
            var root = new XElement(name);
            root.Add(properties);
            return root;
        }
        public static string ConvertArabicToPersianAndViceVersa(string input)
        {
            if (input.Contains("ك") || input.Contains("ي") || input.Contains("ﯼ") || input.Contains("ى") || input.Contains("ة"))
            {
                return input.Replace("ك", "ک").Replace("ي", "ی").Replace("ﯼ", "ی").Replace("ى", "ی").Replace("ة", "ه");
            }
            return input.Replace("ی", "ي").Replace("ک", "ك");
        }
    }
}