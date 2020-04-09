using System.ComponentModel.DataAnnotations;
using Asset.Infrastructure.Common;
using System.Collections.Generic;

namespace Presentation.Panel.Models
{
    public class AppSettingViewModel : BaseViewModel
    {
        
        [Display(Name = "کلید")]
        [Search]
        public string Key { get; set; }

        [Display(Name = "عنوان")]
        [Search]
        public string Title { get; set; }

        [Display(Name = "مقدار")]
        [Search]
        public string Value { get; set; }
        
        [Display(Name = "توضیحات")]
        public string Description { get; set; }
        
        [Display(Name = "آیکن")]
        public string Icon { get; set; }

        [Display(Name = "اولویت")]
        //[Search(GeneralEnums.SearchFieldType.Enum)]
        public int? Priority { get; set; }
        //public string PriorityTitle { get; set; }

        //public List<AppSetting2AppSettingGroupViewModel> AppSetting2Group { get; set; }
        
        //public List<AppSettingGroupViewModel> AppSettingGroup { get; set; }
    }

}