using Asset.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Panel.Models
{
    public class ContentAttachmentViewModel : BaseViewModel
    {
        [Display(Name = "شناسه محتوا")]
        public int? ContentId { get; set; }

        [Display(Name = "عنوان گروه")]
        [Search]
        public string GroupTitle { get; set; }

        [Display(Name = "عنوان")]
        [Search]
        public string Title { get; set; }

        [Display(Name = "توضیحات")]
        [Required(AllowEmptyStrings = true, ErrorMessage = "توضیحات را وارد نمایید")]
        public string Description { get; set; }

        [Display(Name = "مسیر فایل")]
        [Search]
        public string Path { get; set; }

        [Display(Name = "نوع محتوا")]
        [Search]
        public byte? TypeId { get; set; }

        [Display(Name = "نوع محتوا")]
        [Search(GeneralEnums.SearchFieldType.Enum)]
        public string AttachmentTypeTitle { get; set; }

        [Display(Name = "بخش")]
        [Search]
        public byte? PartNo { get; set; }

        [Display(Name = "نمایه")]
        public string Thumbnail { get; set; }

        [Display(Name = "عکس")]
        public string Photo { get; set; }

        [Display(Name = "رایگان")]
        [Search]
        public bool IsFree { get; set; }

        [Display(Name = "اندازه فایل")]
        [Search]
        public int? FileSize { get; set; }

        [Display(Name = "کیفیت")]
        public byte? QualityId { get; set; }

        [Display(Name = "کیفیت")]
        public string QualityTypeTitle { get; set; }

        [Display(Name = "آزمون")]
        public int? ExamId { get; set; }

        [Display(Name = "آزمون")]
        public string ExamTitle { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        [Search(GeneralEnums.SearchFieldType.DateTime)]
        public string CreatedAt { get; set; }

        [Display(Name = "ایجاد کننده")]
        [Search]
        public int? CreatorId { get; set; }

        [Display(Name = "ایجاد کننده")]
        public string CreatorName { get; set; }

        [Display(Name = "آخرین ویرایش کننده")]
        public int? UpdaterId { get; set; }

        [Display(Name = "آخرین ویرایش کننده")]
        public string UpdaterName { get; set; }

        [Display(Name = "تاریخ آخرین ویرایش")]
        public string UpdatedAt { get; set; }

        [Display(Name = "وضعیت")]
        [Required(ErrorMessage = "وضعیت تعیین نشده است")]
        [Search(GeneralEnums.SearchFieldType.Enum)]
        public byte? Status { get; set; }

        [Display(Name = "وضعیت")]
        public string StatusTitle { get; set; }
        public string UniqueId { get; set; }
    }
}