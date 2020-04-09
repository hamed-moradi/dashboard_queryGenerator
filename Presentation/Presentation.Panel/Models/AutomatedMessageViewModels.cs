using Asset.Infrastructure.Common;
using System.ComponentModel.DataAnnotations;
using static Asset.Infrastructure.Common.GeneralEnums;

namespace Presentation.Panel.Models
{
    public class AutomatedMessageViewModel
    {
        public int? Id { get; set; }
        [Search]
        [Display(Name = "کلید یکتا")]
        [Required(ErrorMessage = "کلید یکتا را وارد نمایید")]
        public string UniqueKey { get; set; }
        [Search]
        [Display(Name = "کانال")]
        [Required(ErrorMessage = "کلید کانال را وارد نمایید")]
        public string ChannelKey { get; set; }

        [Search]
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "عنوان را وارد نمایید")]
        public string Title { get; set; }

        [Search]
        [Display(Name = "موضوع")]
        //[Required(ErrorMessage = "موضوع را وارد نمایید")]
        public string Subject { get; set; }

        [Search]
        [Display(Name = "متن")]
        [Required(ErrorMessage = "متن را وارد نمایید")]
        public string Body { get; set; }

        [Search(SearchFieldType.Enum)]
        [Display(Name = "نوع پیام")]
        [Required(ErrorMessage = "نوع پیام را وارد نمایید")]
        public GeneralEnums.AutomatedMessageType? TypeId { get; set; }

        [Display(Name = "نوع پیام")]
        public string TypeTitle { get; set; }

        [Search(SearchFieldType.Enum)]
        [Display(Name = "گروه پیام")]
        [Required(ErrorMessage = "پروه پیام را وارد نمایید")]
        public AutomatedMessageGroup? GroupId { get; set; }

        [Display(Name = "گروه پیام")]
        public string GroupTitle { get; set; }

        [Display(Name = "آخرین ویرایش کننده")]
        public int? UpdaterId { get; set; }

        [Search]
        [Display(Name = "آخرین ویرایش کننده")]
        public string UpdaterName { get; set; }

        [Search(SearchFieldType.DateTime)]
        [Display(Name = "تاریخ آخرین ویرایش")]
        public string UpdatedAt { get; set; }

        [Display(Name = "وضعیت")]
        [Search(SearchFieldType.Enum)]
        public EditStatus? Status { get; set; }

        [Display(Name = "وضعیت")]
        public string StatusTitle { get; set; }
        [Display(Name = "راهنما")]
        [Search]
        public string Hint { get; set; }
        public int ThisPageIndex { get; set; } = 1;
        public string PageOrder { get; set; } = "desc";
        public string PageOrderBy { get; set; } = "id";
    }
}