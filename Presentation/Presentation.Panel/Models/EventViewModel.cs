using System.ComponentModel.DataAnnotations;

namespace Presentation.Panel.Models
{
    public class EventViewModel : BaseViewModel
    {
        //[Search]
        //[Display(Name = "کد")]
        //public string CodeName { get; set; }

        [Search]
        [Display(Name = "عنوان")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "فیلد {0} خالی است")]
        public string Title { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "نمایه")]
        public string Thumbnail { get; set; }

        [Display(Name = "تصویر")]
        public string Image { get; set; }

        [Display(Name = "اولویت")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "فیلد {0} خالی است")]
        public int? Priority { get; set; }

        [Display(Name = "تاریخ شروع")]
        [Search(Asset.Infrastructure.Common.GeneralEnums.SearchFieldType.DateTime)]
        public string StartedAt { get; set; }


        [Display(Name = "تاریخ پایان")]
        [Search(Asset.Infrastructure.Common.GeneralEnums.SearchFieldType.DateTime)]
        public string EndedAt { get; set; }


        [Display(Name = "وضعیت")]
        [Required(ErrorMessage = "وضعیت را وارد نمایید")]
        [Search(Asset.Infrastructure.Common.GeneralEnums.SearchFieldType.Enum)]
        public new EventStatus? Status { get; set; }

        [Display(Name = "وضعیت")]
        public string StatusTitle { get; set; }
    }
}