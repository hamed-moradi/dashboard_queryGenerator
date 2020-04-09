using System.ComponentModel.DataAnnotations;
using static Asset.Infrastructure.Common.GeneralEnums;

namespace Presentation.Panel.Models
{
    public class SentReceivedSMSViewModel:BaseViewModel
    {
        [Required(ErrorMessage = "متن اصلی وارد نشده است.")]
        [Display(Name = "متن اصلی")]
        [Search]
        public string Body { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        [Search(SearchFieldType.DateTime)]
        public string CreatedAt { get; set; }
        [Display(Name = "نوع پیام")]
        [Search(SearchFieldType.Enum)]
        public SmsType? SmsType { get; set; }
        [Display(Name = "ردیف")]
        public int? RowNumber { get; set; }
        [Display(Name = "شماره")]
        public string MSISDN { get; set; }
    }
}