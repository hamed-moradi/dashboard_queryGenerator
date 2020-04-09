using System.ComponentModel.DataAnnotations;

namespace Presentation.Panel.Models
{
    public class Content2TagViewModel : BaseViewModel
    {
        [Display(Name = "محتوا")]
        [Required(ErrorMessage = "محتوا را انتخاب نمایید")]
        public int? ContentId { get; set; }

        [Display(Name = "محتوا")]
        public string ContentTitle { get; set; }

        [Display(Name = "برچسب")]
        [Required(ErrorMessage = "برچسب را انتخاب نمایید")]
        public int? TagId { get; set; }

        [Search]
        [Display(Name = "برچسب")]
        public string TagTitle { get; set; }
    }
}