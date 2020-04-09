using System.Web.Configuration;
using static Asset.Infrastructure.Common.GeneralEnums;

namespace Asset.Infrastructure._App
{
    public class AppSettings
    {
        public static string SmsUserName => WebConfigurationManager.AppSettings["SmsUserName"];
        public static string SmsPassword => WebConfigurationManager.AppSettings["SmsPassword"];
        public static string FcmServerKey => WebConfigurationManager.AppSettings["FcmServerKey"];
        public static string EncryptionKey => WebConfigurationManager.AppSettings["EncryptionKey"];

        public static string CandooSenderNumber => WebConfigurationManager.AppSettings["CandooSenderNumber"];
        public static string CandooSmsUserName => WebConfigurationManager.AppSettings["CandooSmsUserName"];
        public static string CandooSmsPassword => WebConfigurationManager.AppSettings["CandooSmsPassword"];


        private static SMSGeteway _SMSGateway;
        public static SMSGeteway SMSGateway
        {
            get
            {
                if (_SMSGateway == SMSGeteway.Null)
                {
                    var temp = WebConfigurationManager.AppSettings["SMSGateway"];
                    _SMSGateway = Extensions.MapToSMSGateWay(temp);
                }
                return _SMSGateway;
            }
        }
    }
}
