using System.ComponentModel.DataAnnotations;

namespace Presentation.Panel.Models
{
    public class PositionViewModel : ViewModel
    {
        [Search]
        [Display(Name = "عنوان")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "فیلد عنوان خالی است")]
        public string Title { get; set; }
    }
}