using System.ComponentModel.DataAnnotations;


namespace Presentation.Panel.Models
{
    public class SliderViewModel : ViewModel
    {
        [Search]
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "عنوان را وارد نمایید")]
        public string Title { get; set; }

        [Search]
        [Display(Name = "زیرنویس")]
        public string SubTitle { get; set; }

        [Display(Name = "متن")]
        [Required(ErrorMessage = "متن را وارد نمایید")]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }

        [Display(Name = "نمایه")]
        public string Thumbnail { get; set; }

        [Display(Name = "تصویر")]
        public string Image { get; set; }

        [Display(Name = "لینک")]
        [Url(ErrorMessage = "لینک مورد نظر را صحیح وارد نمایید.")]
        public string Link { get; set; }

        [Display(Name = "مکان")]
        [Required(ErrorMessage = "مکان را انتخاب نمایید")]
        public int? PositionId { get; set; }

        [Search]
        [Display(Name = "مکان")]
        public string PositionTitle { get; set; }
    }
}