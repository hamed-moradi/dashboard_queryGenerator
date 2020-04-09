
using Asset.Infrastructure.Common;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Panel.Models
{
    public class BusinessViewModel : ViewModel
    {
        [Search]
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "عنوان را وارد نمایید")]
        public string Title { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }
        [Display(Name = "توضیحات تکمیلی")]
        public string MoreDetails { get; set; }

        [Display(Name = "صاحب")]
        [Required(ErrorMessage = "مالک را وارد نمایید")]
        public int? ProviderId { get; set; }

        [Search]
        [Display(Name = "صاحب")]
        public string ProviderName { get; set; }

        //[Search]
        [Display(Name = "همراه")]
        public string ProviderCell { get; set; }

        [Display(Name = "نمایه")]
        public string Avatar { get; set; }

        [Display(Name = "امتیاز")]
        public decimal? Rate { get; set; }

        [Display(Name = "تعداد امتیازات")]
        public int? RateCount { get; set; }

        //[Search]
        [Display(Name = "تلفن")]
        public string PhoneNo { get; set; }

        //[Search]
        [Display(Name = "فکس")]
        public string Fax { get; set; }

        //[Search]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }

        //[Search]
        [Display(Name = "وب سایت")]
        public string WebSite { get; set; }

        [Display(Name ="محله")]
        public int? GeoId { get; set; }

        [Display(Name = "محله")]
        [Search]
        public string GeoName { get; set; }

        //[Search]
        [Display(Name = "آدرس")]
        public string Address { get; set; }

        [Display(Name = "طول جغرافیایی")]
        [Required(ErrorMessage = "طول جغرافیایی را وارد نمایید")]
        public double? Longitude { get; set; }

        [Display(Name = "عرض جغرافیایی")]
        [Required(ErrorMessage = "عرض جغرافیایی را وارد نمایید")]
        public double? Latitude { get; set; }

        [Display(Name = "شهر")]
        public int? CityId { get; set; }

        [Display(Name = "شهر")]
        public string CityName { get; set; }

        [Display(Name = "وضعیت")]
        [Required(ErrorMessage = "وضعیت را وارد نمایید")]
        [Search(GeneralEnums.SearchFieldType.Enum)]
        public new UserEditStatus? Status { get; set; }
        [Display(Name ="درخواست تغییر")]
        [Search(GeneralEnums.SearchFieldType.Enum)]
        public EditStatus? ChangeRequest { get; set; }
        public bool? EntityHasChanged { get; set; }
        public bool? CategoryHasChanged { get; set; }
        public bool? FacilityHasChanged { get; set; }
        [Display(Name = "وضعیت دسترسی")]
        [Required(ErrorMessage = "وضعیت دسترسی را وارد نمایید")]
        [Search(GeneralEnums.SearchFieldType.Enum)]
        public GeneralEnums.AvilabilityStatus? AvailabilityStatusId { get; set; }

        [Display(Name = "وضعیت دسترسی")]
        public string AvailabilityStatusTitle { get; set; }

        [Display(Name = "وضعیت مدارک")]
        [Required(ErrorMessage = "وضعیت مدارک را وارد نمایید")]
        [Search(GeneralEnums.SearchFieldType.Enum)]
        public GeneralEnums.EvidenceStatusType? EvidenceStatusId { get; set; }

        [Display(Name = "وضعیت مدارک")]
        public string EvidenceStatusTitle { get; set; }

        [Display(Name = "خدمات برای ")]
        public GeneralEnums.Gender? GenderTypeId { get; set; }
        [Display(Name = "خدمات برای ")]
        public string GenderTypeString { get; set; }
        public long? ActivityId { get; set; }
        public GeneralEnums.ServiceRequest2BusinessStatusType? ServiceRequest2BusinessStatus { get; set; }
        [Display(Name = "وضعیت درخواست")]
        public string ServiceRequest2BusinessStatusTitle { get; set; }
        public GeneralEnums.ActionType? ActionTypeId { get; set; }
        public int? ServiceRequestId { get; set; }
        public long? ServiceRequest2BusinessId { get; set; }
        public int PendingAttachmentsCount { get; set; }
        public int PendingEvidenceCount { get; set; }
        public long? RowCountChangeRequest { get; set; }
    }
}