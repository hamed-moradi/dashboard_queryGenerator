using System.ComponentModel.DataAnnotations;
using Asset.Infrastructure.Common;

namespace Presentation.Panel.Models
{
    public class AppSettingGroupViewModel : ViewModel
    {

        public int? Id { get; set; }

        [Display(Name = "عنوان")]
        [Search]
        public string Title { get; set; }
        
        [Display(Name = "توضیحات")]
        public string Description { get; set; }
        
        [Display(Name = "آیکن")]
        public string Icon { get; set; }

        [Display(Name = "والد")]
        //[Search(GeneralEnums.SearchFieldType.Enum)]
        public int? ParentId { get; set; }
        //public string PriorityTitle { get; set; }

    }

}