using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace Asset.Infrastructure._App
{
    public class NotificationModel
    {
        public int? id { get; set; }
        public string title { get; set; }
        public string image { get; set; }
        public string link { get; set; }
        public string description { get; set; }
    }

    public class IosNotificationModel : NotificationModel
    {
        public string body { get; set; }
        public string sound { get; set; }
        public int badge { get; set; }
    }

    public class FcmModel
    {
        public string collapse_key { get; set; }
        public int time_to_live { get; set; }
        public bool delay_while_idle { get; set; }
        public string to { get; set; }
        public string priority { get; set; }
        public bool content_available { get; set; }
        public string[] registration_ids { get; set; }
        public IosNotificationModel data { get; set; }
    }

    public class Fcm
    {
        public string SendNotification(string[] fcmIds, NotificationModel notification)
        {
            var webRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            webRequest.Method = "post";
            webRequest.ContentType = "application/json";
            webRequest.Headers.Add($"Authorization: key={AppSettings.FcmServerKey}");

            var postData = new FcmModel
            {
                registration_ids = fcmIds,
                data = new IosNotificationModel
                {
                    id = notification.id,
                    title = notification.title,
                    body = notification.description,
                    image = notification.image,
                    link = notification.link,
                    sound = "default",
                    badge = 0
                },
                priority = "high",
                content_available = true,
                collapse_key = "score_update",
                delay_while_idle = true,
            };
            var json = new JavaScriptSerializer().Serialize(postData);
            var byteArray = Encoding.UTF8.GetBytes(json);
            webRequest.ContentLength = byteArray.Length;
            var dataStream = webRequest.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            var tResponse = webRequest.GetResponse();

            dataStream = tResponse.GetResponseStream();

            var tReader = new StreamReader(dataStream);

            var sResponseFromServer = tReader.ReadToEnd();

            tReader.Close();
            tResponse.Close();
            dataStream.Close();

            return sResponseFromServer;
        }
    }
}