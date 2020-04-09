using System.ComponentModel.DataAnnotations;

namespace Presentation.Panel.Models
{
    public class AppSetting2AppSettingGroupViewModel : BaseViewModel
    {

        public int? AppSettingGroupId { get; set; }

        public string AppSettingKey { get; set; }

        [Display(Name = "عنوان")]
        public string Title { get; set; }

        [Display(Name = "آیکون")]
        public string Icon { get; set; }

        [Display(Name = "والد")]
        public int? ParentId { get; set; }
        
    }
}