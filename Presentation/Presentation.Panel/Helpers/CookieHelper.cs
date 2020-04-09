using System.Web;
using System.Web.Security;
using Newtonsoft.Json;

namespace Presentation.Panel.Helpers
{
    public class CookieHelper
    {
        public static void Set(string path = null)
        {
            var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null) return;
            var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            if (string.IsNullOrWhiteSpace(authTicket.UserData)) return;
            var serializeModel = JsonConvert.DeserializeObject<CustomPrincipalSerializeModel>(authTicket.UserData);
            var newUser = new CustomPrincipal(authTicket.Name)
            {
                Id = serializeModel.Id,
                FullName = serializeModel.FullName,
                Avatar = serializeModel.Avatar,
                LastLogin = serializeModel.LastLogin,
                IP = serializeModel.IP,
                Path = path
            };
            HttpContext.Current.User = newUser;
        }
    }
}