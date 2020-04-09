
using Asset.Infrastructure.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Panel.Models
{
    public class UserActivityLogViewModel : BaseViewModel
    {
        [Display(Name = "کاربر")]
        public int? UserId { get; set; }

        [Display(Name = "کاربر")]
        [Search]
        public string UserName { get; set; }

        [Display(Name = "کنترلر")]
        [Search]
        public string Controller { get; set; }

        [Display(Name = "اکشن")]
        [Search]
        public string Action { get; set; }

        [Display(Name = "تاریخ شروع")]
        [Search(GeneralEnums.SearchFieldType.DateTime)]
        public string StartedAt { get; set; }

        [Display(Name = "مدت زمان")]
        public string SpentTime { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        [Search(GeneralEnums.SearchFieldType.DateTime)]
        public string CreatedAt { get; set; }
    }
}