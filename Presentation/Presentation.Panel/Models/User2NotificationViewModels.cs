using Asset.Infrastructure.Common;
using System.ComponentModel.DataAnnotations;


namespace Presentation.Panel.Models
{
    public class User2NotificationViewModel : BaseViewModel
    {
        [Display(Name = "کاربر")]
        [Required(ErrorMessage = "کاربر را انتخاب نمایید")]
        public int? UserId { get; set; }

        [Search]
        [Display(Name = "کاربر")]
        public string UserName { get; set; }

        [Display(Name = "اعلان")]
        [Required(ErrorMessage = "اعلان را انتخاب نمایید")]
        public int? NotificationId { get; set; }

        [Display(Name = "اعلان")]
        public string NotificationTitle { get; set; }

        [Display(Name = "وضعیت")]
        public string StateTitle { get; set; }

        [Display(Name = "وضعیت")]
        [Required(ErrorMessage = "اعلان را انتخاب نمایید")]
        [Search(GeneralEnums.SearchFieldType.Enum)]
        public States? State { get; set; }
    }
}