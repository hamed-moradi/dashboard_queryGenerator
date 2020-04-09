using System.ComponentModel.DataAnnotations;

namespace Presentation.Panel.Models
{
    public class MatchGroupViewModel : BaseViewModel
    {
        [Search]
        [Display(Name = "عنوان")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "فیلد {0} خالی است")]
        public string Title { get; set; }

        [Display(Name = "توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "رویداد")]
        [Required(ErrorMessage = "{0} را انتخاب نمایید")]
        public int? EventId { get; set; }

        [Display(Name = "رویداد")]
        [Search]
        public string EventTitle { get; set; }
    }
}