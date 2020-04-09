using Asset.Infrastructure.ir.sms.ip;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Asset.Infrastructure._App
{
    #region ir.sms.ip
    public interface ISmsService
    {
        long[] SendSms(long[] phoneNos, string[] message, ref string response);
    }

    public class SmsService : ISmsService
    {
        #region Private
        private SMSLineNumber[] GetLines(string userName, string password)
        {
            var message = string.Empty;
            var sendReceive = new SendReceive();
            var result = sendReceive.GetSMSLines(userName, password, ref message);
            if (!string.IsNullOrWhiteSpace(message))
            {
                throw new Exception(message);
            }
            return result;
        }

        private void MessageValidation(long[] phoneNos, string[] message)
        {
            if (phoneNos == null || message == null)
            {
                throw new Exception("شماره و پیام را وارد نمایید.");
            }

            var messagesCount = message.Count();
            var numbersCount = phoneNos.Count();

            if (messagesCount != 1 && numbersCount != messagesCount)
            {
                throw new Exception("تعداد شماره ها و پیام ها باهم همخوانی ندارد.");
            }
        }
        #endregion

        public long[] SendSms(long[] phoneNos, string[] message, ref string response)
        {
            MessageValidation(phoneNos, message);

            long[] result;
            int smsLineId = 0;
            var messagesCount = message.Count();
            var numbersCount = phoneNos.Count();

            var details = new List<WebServiceSmsSend>();
            for (int i = 0; i < numbersCount; i++)
            {
                details.Add(new WebServiceSmsSend
                {
                    IsFlash = false,
                    MessageBody = messagesCount > 1 ? message[i] : message[0],
                    MobileNo = phoneNos[i]
                });
            }
            var userName = AppSettings.SmsUserName;
            var password = AppSettings.SmsPassword;
            var smsLines = GetLines(userName, password);
            if (smsLines != null && smsLines.Count() > 0)
            {
                smsLineId = smsLines[0].ID;
            }
            else
            {
                throw new Exception("هیچ خطی برای ارسال پیام یافت نشد.");
            }
            var sendReceive = new SendReceive();
            result = sendReceive.SendMessage(userName, password, details.ToArray(), smsLineId, DateTime.Now, ref response);
            if (!string.IsNullOrWhiteSpace(response))
            {
                throw new Exception(response);
            }
            return result;
        }
    }
    #endregion

    #region candooSms
    public interface ICandooSmsService
    {
        void SendSms(long[] phoneNos, string message);
    }
    public class CandooSmsService : ICandooSmsService
    {
        public void SendSms(long[] phoneNos, string message)
        {
            try
            {
                using (var service = new com.candoosms.panel.SMSAPI())
                {
                    var userName = AppSettings.CandooSmsUserName;
                    var password = AppSettings.CandooSmsPassword;
                    var senderNumber = AppSettings.CandooSenderNumber;
                    var lstNumbers = phoneNos.Select(x => x.ToString().Insert(0, "98")).ToArray();
                    com.candoosms.panel.SendResult[] result = service.Send(userName, password, senderNumber, message, lstNumbers, "0");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
    #endregion
}
