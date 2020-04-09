using Asset.Infrastructure.Common;
using System.ComponentModel.DataAnnotations;


namespace Presentation.Panel.Models
{
    public class UserMatchPointViewModel : BaseViewModel
    {
        [Display(Name = "نام کاربری")]
        public string UserName { get; set; }

        [Display(Name = "شناسه کاربری")]
        public int? UserId { get; set; }

        [Display(Name = "شناسه رویداد")]
        public int EventId { get; set; }

        [Display(Name = "شناسه رویداد")]
        public string EventTitle { get; set; }

        [Display(Name = "آواتار")]
        public string Avatar { get; set; }

        [Display(Name = "مجموع امتیازات")]
        public int TotalPoints { get; set; }
    }
}