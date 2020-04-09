using System.ComponentModel.DataAnnotations;

namespace Presentation.Panel.Models
{
    public class TagViewModel : ViewModel
    {
        [Search]
        [Display(Name = "عنوان")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "فیلد عنوان خالی است")]
        public string Title { get; set; }

        [Display(Name = "اولویت")]
        [Required(ErrorMessage = "فیلد اولویت خالی است")]
        public int? Priority { get; set; }
    }
}