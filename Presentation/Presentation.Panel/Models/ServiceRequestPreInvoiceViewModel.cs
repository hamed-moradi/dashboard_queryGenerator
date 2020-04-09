using System.ComponentModel.DataAnnotations;
using static Asset.Infrastructure.Common.GeneralEnums;

namespace Presentation.Panel.Models
{
    public class ServiceRequestPreInvoiceViewModel:BaseViewModel
    {
        public long? ServiceRequest2BusinessId { get; set; }
        public int? ServiceRequestId { get; set; }
        public int? ProviderId { get; set; }
        public int? BusinessId { get; set; }
        public int? CustomerId { get; set; }
        public int? CustomerVehicleId { get; set; }
        [Display(Name ="توضیحات")]
        public string Description { get; set; }
        [Display(Name = "تاریخ مقرر")]
        public string  DueDate { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        public string CreatedAt { get; set; }
        [Display(Name = "تاریخ آخرین ویرایش")]
        public string LastModifiedAt { get; set; }
        [Display(Name = "مبلغ نهایی")]
        public decimal? TotalPrice { get; set; }
        [Display(Name = "وضعیت")]
        [Search(SearchFieldType.Enum)]
        public ServiceRequestPreInvoiceStatusType? Status { get; set; }
        [Display(Name = "وضعیت")]
        public string StatusTitle { get; set; }
        public int? CreatorId { get; set; }
        [Display(Name = "نام خدمات دهنده")]
        public string ProviderName { get; set; }
        [Display(Name = "عنوان کسب و کار")]
        public string BusinessTitle { get; set; }
        [Display(Name = "نام مشتری")]
        [Search]
        public string CustomerName { get; set; }
        [Display(Name = "عنوان خودرو")]
        [Search]
        public string CustomerVehicleTitle { get; set; }
        [Display(Name ="ادمین ایجاد کننده")]
        public string CreatorName { get; set; }
    }
}