using System.ComponentModel.DataAnnotations;

namespace Presentation.Panel.Models
{
    public class AutomatedMessageParameterViewModel
    {
        [Display(Name = "عنوان")]
        public string Title { get; set; }
        [Display(Name = "کلید")]
        public string Key { get; set; }
        [Display(Name = "توضیحات")]
        public string Description { get; set; }
    }
}