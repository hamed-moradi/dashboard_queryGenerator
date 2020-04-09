using System.ComponentModel.DataAnnotations;

namespace Presentation.Panel.Models
{
    public class ClubViewModel : BaseViewModel
    {
        [Search]
        [Display(Name = "نام")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "فیلد {0} خالی است")]
        public string Name { get; set; }

        [Search]
        [Display(Name = "خلاصه")]
        [StringLength(4, ErrorMessage = "{0} باید حداقل {2} و حداکثر 4 کارکتر باشد.", MinimumLength = 2)]
        public string Abbreviation { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "نمایه")]
        public string Thumbnail { get; set; }

        [Display(Name = "تصویر")]
        public string Image { get; set; }
        
        [Display(Name = "وضعیت")]
        [Required(ErrorMessage = "وضعیت را وارد نمایید")]
        [Search(Asset.Infrastructure.Common.GeneralEnums.SearchFieldType.Enum)]
        public new ClubStatus? Status { get; set; }

        [Display(Name = "وضعیت")]
        public string StatusTitle { get; set; }


        [Display(Name = "رویداد")]
        [Required(ErrorMessage = "{0} را انتخاب نمایید")]
        public int? EventId { get; set; }

        [Display(Name = "رویداد")]
        [Search]
        public string EventTitle { get; set; }
    }
}