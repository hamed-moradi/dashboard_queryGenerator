using Asset.Infrastructure.Common;
using System.ComponentModel.DataAnnotations;
using System.Web;
using static Asset.Infrastructure.Common.GeneralEnums;

namespace Presentation.Panel.Models
{
    public class UploadedPinFactorViewModel:BaseViewModel
    {
        public new long? Id { get; set; }
        [Search]
        [StringLength(512)]
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "عنوان را وارد نمائید.")]
        public string Title { get; set; }
        [Search]
        [Required(ErrorMessage = "توضیحات را وارد نمائید.")]
        [Display(Name = "توضیحات")]
        public string Description { get; set; }
        [Search]
        [Display(Name = "شماره فاکتور")]
        [Required(ErrorMessage = "شماره فاکتور را وارد نمائید.")]
        [StringLength(128)]
        public string FactorNo { get; set; }
        [Required(ErrorMessage = "قیمت را وارد نمائید.")]
        [Search(SearchFieldType.Number)]
        [Display(Name = "قیمت (ریال)")]
        public long? Price { get; set; }
        [Required(ErrorMessage = "تعداد را وارد نمائید.")]
        [Search(SearchFieldType.Number)]
        [Display(Name = "تعداد")]
        public int? Quantity { get; set; }
        [Required(ErrorMessage = "اپراتور را مشخص نمائید.")]
        [Search(SearchFieldType.Enum)]
        [Display(Name = "اپراتور")]
        public GeneralEnums.OperatorType? OpTypeId { get; set; }
        [Required(ErrorMessage = "نوع پین را مشخص نمائید.")]
        [Search(SearchFieldType.Enum)]
        [Display(Name = "نوع پین (ریال)")]
        public PinType? PinTypeId { get; set; }
        [Required(ErrorMessage = "نوع شارژ را مشخص نمائید.")]
        [Search(SearchFieldType.Enum)]
        [Display(Name = "نوع ورودی")]
        public PinInputType? InputTypeId { get; set; }
        [Display(Name = "نام فایل")]
        [StringLength(512)]
        public string Filename { get; set; }
        [Display(Name = "شماره رفرنس")]
        public long? ReferenceNumber { get; set; }
        [Display(Name = "کاربر")]
        public int? CreatorId { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        public string CreatedAt { get; set; }
        [Required(ErrorMessage = "وضعیت فاکتور شارژ را مشخص نمائید.")]
        [Search(SearchFieldType.Enum)]
        [Display(Name = "وضعیت")]
        public PinFactorStatus? Status { get; set; }
        [Display(Name = "وضعیت")]
        public string StatusString { get; set; }
        [Display(Name = "کاربر")]
        public string CreatorName { get; set; }
        [Required(ErrorMessage = "فایل را انتخاب نمائید.")]
        [Display(Name = "فایل")]
        public HttpPostedFileBase File { get; set; }
    }
}