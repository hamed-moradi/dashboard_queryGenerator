using Asset.Infrastructure.Common;
using System.ComponentModel.DataAnnotations;


namespace Presentation.Panel.Models
{
    public class ModuleViewModel : BaseViewModel
    {
        [Search]
        [Display(Name = "عنوان")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "فیلد عنوان خالی است")]
        public string Title { get; set; }

        [Search]
        [Display(Name = "مسیر")]
        [Required(ErrorMessage = "فیلد مسیر خالی است")]
        public string Path { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "آیکن")]
        public string Icon { get; set; }

        [Display(Name = "بالاسری")]
        public int? ParentId { get; set; }

        [Search]
        [Display(Name = "بالاسری")]
        public string ParentTitle { get; set; }

        [Display(Name = "متد")]
        public string HttpMethod { get; set; }

        [Display(Name = "گروه")]
        [Search(GeneralEnums.SearchFieldType.Enum)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "فیلد دسته بندی خالی است")]
        public GeneralEnums.ModuleCategory? Category { get; set; }

        [Display(Name = "اولویت")]
        public byte? Priority { get; set; }

        [Display(Name = "وضعیت")]
        [Required(ErrorMessage = "وضعیت را وارد نمایید")]
        [Search(GeneralEnums.SearchFieldType.Enum)]
        public EditStatus? Status { get; set; }

        [Display(Name = "وضعیت")]
        public string StatusTitle { get; set; }
    }
}