using Asset.Infrastructure.Common;
using System.ComponentModel.DataAnnotations;
using static Asset.Infrastructure.Common.GeneralEnums;

namespace Presentation.Panel.Models
{
    public class UploadedPinFactorItemArchiveViewModel:BaseViewModel
    {
        public new long? Id { get; set; }
        public int? CampaignId { get; set; }
        public int? AwardCheckoutId { get; set; }
        [Display(Name = "فاکتور")]
        public int? FactorId { get; set; }
        [Search]
        [Display(Name = "فاکتور")]
        public string FactorTitle { get; set; }
        [Display(Name = "اپراتور")]
        [Search(SearchFieldType.Enum)]
        public GeneralEnums.OperatorType? OpTypeId { get; set; }
        [Display(Name = "نوع پین (ریال)")]
        [Search(SearchFieldType.Enum)]
        public GeneralEnums.PinType? PinTypeId { get; set; }
        [Display(Name = "شماره سریال")]
        [Search]
        public string SerialNo { get; set; }
        [Display(Name = "پین کد")]
        [Search]
        public string PinCode { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        [Search(SearchFieldType.DateTime)]
        public string CreatedAt { get; set; }

        [Display(Name = "تاریخ آخرین ویرایش")]
        public string UpdatedAt { get; set; }
        [Display(Name = "وضعیت")]
        [Search(SearchFieldType.Enum)]
        public GeneralEnums.FactorItemStatus? Status { get; set; }
        [Display(Name = "وضعیت")]
        public string StatusString { get; set; }
    }
}