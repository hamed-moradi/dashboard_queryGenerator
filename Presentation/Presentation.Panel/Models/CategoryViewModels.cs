using Asset.Infrastructure.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Panel.Models
{
    public class CategoryViewModel : ViewModel
    {
        [Search]
        [Display(Name = "عنوان")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "فیلد عنوان خالی است")]
        public string Title { get; set; }

        [Display(Name = "وضعیت")]
        [Required(ErrorMessage = "وضعیت را وارد نمایید")]
        [Search(GeneralEnums.SearchFieldType.Enum)]
        public new CategoryEditStatus? Status { get; set; }

        [Search]
        [Display(Name = "توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "اولویت")]
        [Required(ErrorMessage = "فیلد اولویت خالی است")]
        public int? Priority { get; set; }

        [Display(Name = "عکس")]
        public string Photo { get; set; }

        [Display(Name = "نمایه")]
        public string Thumbnail { get; set; }

        [Display(Name = "والد")]
        public int? ParentId { get; set; }

        [Search]
        [Display(Name = "والد")]
        public string ParentTitle { get; set; }

        [Display(Name = "دسته بندی ها")]
        public List<TreeModel> CategoryTree { get; set; }
    }
}