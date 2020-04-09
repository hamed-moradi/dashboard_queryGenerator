using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Panel.Models
{
    public class Category2BusinessViewModel : BaseViewModel
    {
        [Display(Name = "دسته بندی")]
        [Required(ErrorMessage = "دسته بندی را وارد نمایید")]
        public int? CategoryId { get; set; }

        [Display(Name = "دسته بندی")]
        public string CategoryTitle { get; set; }

        [Display(Name = "خدمات دهنده")]
        //[Required(ErrorMessage = "خدمات دهنده را وارد نمایید")]
        public int? BusinessId { get; set; }

        [Search]
        [Display(Name = "خدمات دهنده")]
        public string BusinessTitle { get; set; }
        public long? ActivityId { get; set; }
    }
}