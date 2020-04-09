using System.ComponentModel.DataAnnotations;
using Asset.Infrastructure.Common;

namespace Presentation.Panel.Models
{
    public class AppVersionViewModel : ViewModel
    {
        [Display(Name = "کلاینت")]
        [Search]
        public int? ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientAvatar { get; set; }

        [Display(Name = "نسخه")]
        [Search]
        public string Version { get; set; }

        [Display(Name = "Major")]
        public byte? Major { get; set; }

        [Display(Name = "Minor")]
        public byte? Minor { get; set; }

        [Display(Name = "Patch")]
        [Search]
        public byte? Patch { get; set; }

        [Display(Name = "وضعیت بروزرسانی")]
        [Search(GeneralEnums.SearchFieldType.Enum)]
        [Required(ErrorMessage = "وضعیت بروزرسانی را انتخاب کنید نمایید")]
        public GeneralEnums.ForceUpdateState? ForceUpdateRequired { get; set; }
        public string ForceUpdateRequiredTitle { get; set; }

        [Display(Name = "آدرس استور")]
        public string UpdateStoreUrl { get; set; }

        [Display(Name = "آدرس لوکال")]
        public string UpdateLocalUrl { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        //[Display(Name = "تاریخ ایجاد")]
        //[Search(GeneralEnums.SearchFieldType.DateTime)]
        //public string CreatedAt { get; set; }

        //[Display(Name = "ایجاد کننده")]
        //[Search]
        //public int? CreatorId { get; set; }
        //public string CreatorName { get; set; }

        //[Display(Name = "آخرین ویرایش کننده")]
        //public int? UpdaterId { get; set; }
        //public string UpdaterName { get; set; }

        [Display(Name = "تاریخ آخرین ویرایش")]
        public string LastUpdatedAt { get; set; }

        [Display(Name = "وضعیت")]
        //[Required(ErrorMessage = "وضعیت تعیین نشده است")]
        [Search(GeneralEnums.SearchFieldType.Enum)]
        public new GeneralEnums.SubStatus? Status { get; set; }
        public string StatusName { get; set; }

        public int? LastStableVersionId { get; set; }

        [Display(Name = "انتصاب به عنوان آخرین ورژن")]
        public bool LastVersionCheck { get; set; }

    }

}