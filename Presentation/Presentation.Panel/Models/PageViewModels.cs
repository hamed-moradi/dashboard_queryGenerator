using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Presentation.Panel.Models
{
    public class PageViewModel : ViewModel
    {
        [Display(Name = "نام")]
        [Search]
        [Required(AllowEmptyStrings = false, ErrorMessage = "فیلد نام خالی است")]
        public string UniqueName { get; set; }

        [Display(Name = "عنوان")]
        [Search]
        [Required(AllowEmptyStrings = false, ErrorMessage = "فیلد عنوان خالی است")]
        public string Title { get; set; }

        [Display(Name = "زیرنویس")]
        [Search]
        public string SubTitle { get; set; }

        [Display(Name = "خلاصه")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "فیلد خلاصه خالی است")]
        [DataType(DataType.MultilineText)]
        public string Summary { get; set; }

        [Display(Name = "بدنه")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "فیلد بدنه خالی است")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Body { get; set; }

        [Display(Name = "نمایه")]
        public string Thumbnail { get; set; }

        [Display(Name = "بنر")]
        public string Photo { get; set; }
    }
}