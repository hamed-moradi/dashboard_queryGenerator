using Asset.Infrastructure.Common;
using System.ComponentModel.DataAnnotations;


namespace Presentation.Panel.Models
{
    public class MessageViewModel : BaseViewModel
    {
        [Display(Name = "نام کاربر")]
        [Search]
        public string Name { get; set; }

        [Display(Name = "عنوان پیام")]
        [Search]
        public string Title { get; set; }

        [Display(Name = "تلفن")]
        [Search]
        public string CellPhone { get; set; }

        [Display(Name = "ایمیل")]
        [Search]
        public string Email { get; set; }

        [Display(Name = "متن پیام")]
        public string Body { get; set; }

        [Display(Name = "کاربر")]
        public int? UserId { get; set; }

        [Display(Name = "کاربر")]
        [Search]
        public string UserName { get; set; }

        [Display(Name = "آخرین بیننده")]
        public int? LastViewerId { get; set; }

        [Display(Name = "آخرین بیننده")]
        [Search]
        public string LastViewer { get; set; }

        [Display(Name = "با اهمیت؟")]
        [Required(ErrorMessage = "فیلد اهمیت را انتخاب نمایید")]
        [Search(GeneralEnums.SearchFieldType.Boolean)]
        public bool? IsImportant { get; set; }

        [Display(Name = "تاریخ آخرین نمایش")]
        [Search(GeneralEnums.SearchFieldType.DateTime)]
        public string LastSeenAt { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        [Search(GeneralEnums.SearchFieldType.DateTime)]
        public string CreatedAt { get; set; }

        [Display(Name = "وضعیت")]
        [Search(GeneralEnums.SearchFieldType.Enum)]
        public MessageStatus? Status { get; set; }

        [Display(Name = "وضعیت")]
        public string StatusTitle { get; set; }
    }
}