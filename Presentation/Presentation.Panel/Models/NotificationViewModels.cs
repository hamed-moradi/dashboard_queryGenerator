
using Asset.Infrastructure.Common;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Panel.Models
{
    public class NotificationViewModel : BaseViewModel
    {
        [Display(Name = "عنوان")]
        [Search]
        public string Title { get; set; }

        [Display(Name = "متن")]
        [Search]
        public string Body { get; set; }

        [Display(Name = "مرجع")]
        public int? RefrenceId { get; set; }

        [Display(Name = "مرجع")]
        public byte? RefrenceTypeId { get; set; }

        [Display(Name = "تعداد کاربر")]
        public int? UserCount { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        [Search(GeneralEnums.SearchFieldType.DateTime)]
        public string CreatedAt { get; set; }

        [Display(Name = "ایجاد کننده")]
        public int? CreatorId { get; set; }

        [Display(Name = "ایجاد کننده")]
        public string CreatorName { get; set; }

        [Display(Name = "تاریخ نمایش")]
        [Search(GeneralEnums.SearchFieldType.DateTime)]
        public string ShowAt { get; set; }

        [Display(Name = "وضعیت")]
        [Required(ErrorMessage = "وضعیت تعیین نشده است")]
        [Search(GeneralEnums.SearchFieldType.Enum)]
        public EditStatus? Status { get; set; }

        [Display(Name = "وضعیت")]
        public string StatusTitle { get; set; }
    }
}