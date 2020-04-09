using System.ComponentModel.DataAnnotations;


namespace Presentation.Panel.Models
{
    public class ReviewViewModel : ViewModel
    {
        [Display(Name = "امتیاز")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "فیلد امتیاز خالی است")]
        public decimal? Point { get; set; }

        [Display(Name = "متن")]
        public string Body { get; set; }

        [Display(Name = "خدمات دهنده")]
        public int? ProviderId { get; set; }

        [Search]
        [Display(Name = "خدمات دهنده")]
        public string BusinessTitle { get; set; }

        [Display(Name = "آی پی")]
        public string IP { get; set; }
        public int? BusinessId { get; set; }
    }
}