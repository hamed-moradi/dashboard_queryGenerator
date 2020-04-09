using Asset.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Panel.Models
{
    public class BusinessAttachmentViewModels : BaseViewModel
    {
        [Display(Name = "شناسه کسب و کار")]
        public int? BusinessId { get; set; }

        [Display(Name = "عنوان")]
        [Search]
        public string Title { get; set; }

        [Display(Name = "توضیحات")]
        
        public string Description { get; set; }

        [Display(Name = "نوع محتوا")]
        [Search(GeneralEnums.SearchFieldType.Enum)]
        [Required(ErrorMessage = "{0} را وارد نمایید")]
        public GeneralEnums.AttachmentType? TypeId { get; set; }

        [Display(Name = "نوع محتوا")]
        [Search(GeneralEnums.SearchFieldType.Enum)]
        public string TypeTitle { get; set; }

        [Display(Name = "مسیر فایل")]
        [Required(ErrorMessage = "{0} را وارد نمایید")]
        public string Path { get; set; }

        [Display(Name ="حجم فایل")]
        public int? Size { get; set; }
        [Display(Name = "مدت ویدیو")]
        public long? Duration { get; set; }
        [Display(Name = "فرمت فایل")]
        public string Extension { get; set; }
        [Display(Name = "کیفیت")]
        public GeneralEnums.QulaityType? QualityId { get; set; }

        [Display(Name = "کیفیت")]
        public string QualityTypeTitle { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        [Search(GeneralEnums.SearchFieldType.DateTime)]
        public string CreatedAt { get; set; }

        [Display(Name = "ادمین ایجاد کننده")]
        public int? CreatorId { get; set; }

        [Display(Name = "ادمین ایجاد کننده")]
        public string CreatorName { get; set; }

        [Display(Name = "آخرین ادمین ویرایش کننده")]
        public int? UpdaterId { get; set; }

        [Display(Name = "آخرین ادمین ویرایش کننده")]
        public string UpdaterName { get; set; }

        [Display(Name = "تاریخ آخرین ویرایش")]
        public string UpdatedAt { get; set; }

        [Display(Name = "وضعیت")]
        [Required(ErrorMessage = "{0} را وارد نمایید")]
        [Search(GeneralEnums.SearchFieldType.Enum)]
        public GeneralEnums.Status? Status { get; set; }

        [Display(Name = "وضعیت")]
        public string StatusTitle { get; set; }
    }
}