using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Panel.Models
{
    public class Category2FacilityViewModel : BaseViewModel
    {
        [Display(Name = "دسته بندی")]
        [Required(ErrorMessage = "دسته بندی را وارد نمایید")]
        public int? CategoryId { get; set; }

        [Display(Name = "دسته بندی")]
        public string CategoryTitle { get; set; }

        [Display(Name = "سرویس")]
        //[Required(ErrorMessage = "ویژگی را وارد نمایید")]
        public int? FacilityId { get; set; }

        [Search]
        [Display(Name = "سرویس")]
        public string FacilityTitle { get; set; }
    }
}