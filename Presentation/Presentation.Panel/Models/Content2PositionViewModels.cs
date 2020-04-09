using System.ComponentModel.DataAnnotations;

namespace Presentation.Panel.Models
{
    public class Content2PositionViewModel : BaseViewModel
    {
        [Display(Name = "محتوا")]
        [Required(ErrorMessage = "محتوا را انتخاب نمایید")]
        public int? ContentId { get; set; }

        [Display(Name = "محتوا")]
        public string ContentTitle { get; set; }

        [Display(Name = "برچسب")]
        [Required(ErrorMessage = "برچسب را انتخاب نمایید")]
        public int? PositionId { get; set; }

        [Search]
        [Display(Name = "برچسب")]
        public string PositionTitle { get; set; }
    }
}