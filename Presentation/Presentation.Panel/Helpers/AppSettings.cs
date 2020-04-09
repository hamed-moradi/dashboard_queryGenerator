using System.Web.Configuration;

namespace Presentation.Panel.Helpers
{
    public class AppSettings
    {
        public static string TelegramChannel => WebConfigurationManager.AppSettings["TelegramChannel"];
    }
}